using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

using ZoneFiveSoftware.Common.Visuals.Fitness;
using GK.AttackPoint;
using System.Xml.Serialization;
using System.IO;
using System.Diagnostics;
using ZoneFiveSoftware.Common.Data.Fitness;
using System.Runtime.Serialization.Formatters.Binary;
using GK.SportTracks.AttackPoint.UI.Activities;
using ZoneFiveSoftware.Common.Data.Measurement;
using System.Windows.Forms;
using System.Reflection;
using GK.SportTracks.AttackPoint.Properties;

namespace GK.SportTracks.AttackPoint
{
    class ApPlugin : IPlugin
    {
        private const string ApConstantDataFileName = "ap-constant-data.xml";
        public const string FeedbackEmail = "gregory.kh+st2ap@gmail.com";

        private static IApplication _application;
        private static Guid PluginId = new Guid("{1eec167a-0605-479e-8def-08633ac68a22}");
        private static ApProxy Proxy;
        private static XmlWriterSettings DataSerSettings;
        private static string BasePath;
        private static string LogFile;

        static ApPlugin() {
            try {
                DataSerSettings = new XmlWriterSettings();
                DataSerSettings.OmitXmlDeclaration = true;
                DataSerSettings.Indent = false;
                BasePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"ZoneFiveSoftware\SportTracks");
                if (!Directory.Exists(path)) {
                    Directory.CreateDirectory(path);
                }

                LogFile = Path.Combine(path, "attackpoint-plugin.log");
            }
            catch (Exception ex) {
                MessageBox.Show(string.Format("Unable to initialize AttckPoint plugin. Please uninstall it and report this error to {0}.{2}{1}", FeedbackEmail, ex, Environment.NewLine));
            }
        }

        public IApplication Application { set { _application = value; } }
        public static ApConfig ApConfig { get; set; }
        public Guid Id { get { return PluginId; } }
        public string Name { get { return Resources.Plugin_Name; } }

        public static ApProxy GetProxy() {
            if (string.IsNullOrEmpty(ApConfig.Profile.Username) || string.IsNullOrEmpty(ApConfig.Profile.Password))
                throw new ApplicationException(Resources.Error_ApCredentialsNotSpecified);

            if (ApConfig.Profile.CredentialsChanged || (Proxy == null || Proxy.Expired)) {
                Proxy = ApProxy.Connect(LogFile, BasePath, ApConfig.Profile.Username, ApConfig.Profile.Password);
            }

            return Proxy;
        }

        public void ReadOptions(XmlDocument xmlDoc, XmlNamespaceManager nsmgr, XmlElement pluginNode)
        {
            try {
                ApConfig = null;
                var innerXml = pluginNode.InnerXml;
                if (!string.IsNullOrEmpty(innerXml)) {
                    var ser = new XmlSerializer(typeof(ApConfig));
                    using (var reader = new StringReader(innerXml)) {
                        ApConfig = (ApConfig)ser.Deserialize(reader);
                    }
                }

                if (ApConfig == null) {
                    ApConfig = new ApConfig();
                }

                if (ApConfig.Profile == null) {
                    ApConfig.Profile = new ApProfile();
                }

                var ser2 = new XmlSerializer(typeof(ApConstantData));
                using (var reader = new StreamReader(Path.Combine(BasePath, ApConstantDataFileName))) {
                    ApConfig.Profile.ConstantData = (ApConstantData)ser2.Deserialize(reader);
                }

                if (ApConfig.Mapping == null) {
                    ApConfig.Mapping = new ApMapping();
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Unable to read AttackPoint config: " + ex);
                Debug.WriteLine("Unable to read AttackPoint config: " + ex);
            }
        }

        public string Version
        {
            get { return GetType().Assembly.GetName().Version.ToString(3); }
        }

        public void WriteOptions(XmlDocument xmlDoc, XmlElement pluginNode)
        {
            try {
                var ser = new XmlSerializer(typeof(ApConfig));
                var settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;
                var sb = new StringBuilder();
                using (var writer = XmlWriter.Create(sb, settings)) {
                    ser.Serialize(writer, ApConfig);
                }

                pluginNode.InnerXml = sb.ToString();
            }
            catch (Exception ex) {
                Debug.WriteLine("Unable to write AttackPoint config: " + ex);
            }
        }

        public static IApplication GetApplication()
        {
            return _application;
        }

        internal static List<StIntensity> GetStIntensities() {
            if (ApConfig.Mapping.Intensities == null || ApConfig.Mapping.Intensities.Count == 0) {
                ApConfig.Mapping.Intensities = new List<StIntensity>();
                for (int i = 0; i <= 10; ++i) {
                    ApConfig.Mapping.Intensities.Add(
                        new StIntensity() { StId = i.ToString() });
                }
            }

            return ApConfig.Mapping.Intensities;
        }

        internal static List<StEquipment> GetStEquipmentItems() {
            var equipment = _application.Logbook.Equipment;

            List<StEquipment> stEquipment = null;

            if (ApConfig.Mapping.Shoes == null) {
                ApConfig.Mapping.Shoes = new List<StEquipment>();
            }
            else {
                stEquipment = new List<StEquipment>(ApConfig.Mapping.Shoes);
            }

            ApConfig.Mapping.Shoes.Clear();

            foreach (var item in equipment) {
                StEquipment e = null;
                if ((stEquipment == null) ||
                    (e = stEquipment.Find(c => c.StId == item.ReferenceId)) == null) {
                    e = new StEquipment(item);
                }
                else {
                    ((StEquipment)e).Instance = item;
                }

                ApConfig.Mapping.Shoes.Add(e);
            }
            return ApConfig.Mapping.Shoes;
        }

        internal static List<StCategory> GetStCategories() {
            var categories = _application.Logbook.ActivityCategories;
            var list = new List<IActivityCategory>();
            foreach (IActivityCategory cat in categories) {
                // HACK: I assume here that the first root category is the main one
                // I skip all other root categories like "My friends"
                foreach (IActivityCategory subcat in cat.SubCategories) {
                    FlattenSportTrackActivityTypes(list, subcat);
                }
                break;
            }

            List<StCategory> stCategories = null;

            if (ApConfig.Mapping.Activities == null) {
                ApConfig.Mapping.Activities = new List<StCategory>();
            }
            else {
                stCategories = new List<StCategory>(ApConfig.Mapping.Activities);
            }

            ApConfig.Mapping.Activities.Clear();

            foreach (var category in list) {
                StCategory cat = null;
                if ((stCategories == null) ||
                    (cat = stCategories.Find(c => c.StId == category.ReferenceId)) == null) {
                    cat = new StCategory(category);
                }
                else {
                    ((StCategory)cat).Instance = category;
                }

                ApConfig.Mapping.Activities.Add(cat);
            }


            return ApConfig.Mapping.Activities;
        }

        private static void FlattenSportTrackActivityTypes(List<IActivityCategory> list, IActivityCategory category) {
            list.Add(category);
            foreach (IActivityCategory sub in category.SubCategories) {
                FlattenSportTrackActivityTypes(list, sub);
            }
        }


        internal static void SaveApData(IActivity activity, ApActivityData data) {
            SerializeApActivityData(data);

            byte[] payload = null;
            if (data != null && !data.IsEmpty()) {
                var ser = new XmlSerializer(typeof(ApActivityData));
                var sb = new StringBuilder();
                using (var writer = XmlWriter.Create(sb, DataSerSettings)) {
                    ser.Serialize(writer, data);
                }
                payload = new UTF8Encoding().GetBytes(sb.ToString());
            }

            activity.SetExtensionData(PluginId, payload);
            ApPlugin.GetApplication().Logbook.Modified = true;
        }

        internal static ApActivityData GetApData(IActivity activity) {
            ApActivityData data = null;
            try {
                byte[] bytes = activity.GetExtensionData(ApPlugin.PluginId);
                if (bytes != null && bytes.Length > 0) {
                    var s = new UTF8Encoding().GetString(bytes);
                    var ser = new XmlSerializer(typeof(ApActivityData));
                    using (var reader = new StringReader(s)) {
                        data = (ApActivityData)ser.Deserialize(reader);
                    }
                }
                else {
                    data = new ApActivityData();
                }
            }
            catch (Exception ex) {
                throw ex;
            }

            SerializeApActivityData(data);

            return data;
        }

        private static void SerializeApActivityData(ApActivityData data) {
            if (data == null) return;
            if (data.IsEmpty()) {
                Debug.WriteLine("Data is empty!");
                return;
            }
            var sb = new StringBuilder();
            using (var w = new StringWriter(sb)) {
                var ser = new XmlSerializer(typeof(ApActivityData));
                ser.Serialize(w, data);
            }
            Debug.WriteLine(sb);
        }

        internal static Units GetDistanceUnits() {
            var units = GetApplication().SystemPreferences.DistanceUnits;
            return units == Length.Units.Centimeter || units == Length.Units.Kilometer || units == Length.Units.Meter ? Units.Metric : Units.English;
        }
    }
}
