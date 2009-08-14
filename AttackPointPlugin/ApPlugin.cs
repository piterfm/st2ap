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
using System.Web;

namespace GK.SportTracks.AttackPoint
{
    class ApPlugin : IPlugin
    {
        public const string FeedbackEmail = "gregory.kh+st2ap@gmail.com";
        public const string HomePage = "http://st2ap.codeplex.com";
        public const string DocPage = "http://st2ap.codeplex.com/Wiki/View.aspx?title=Getting%20Started";

        private static IApplication _application;
        private static bool _initialized;
        private static Guid PluginId = new Guid("{1eec167a-0605-479e-8def-08633ac68a22}");
        private static ApProxy Proxy;
        private static XmlWriterSettings DataSerSettings;
        private static string BasePath;

        static ApPlugin() {
            try {
                LogManager.Logger = new Logger();
                DataSerSettings = new XmlWriterSettings();
                DataSerSettings.OmitXmlDeclaration = true;
                DataSerSettings.Indent = false;
                BasePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                ApConfig = new ApConfig(BasePath);
                _initialized = true;
            }
            catch (Exception ex) {
                MessageBox.Show("Unable to initialize AttackPoint plugin. Please see log for details.");
                if (LogManager.Logger != null) {
                    Logger.LogMessage("Failed to initialize AP plugin.", ex);
                }
            }
        }

        public static ILogger Logger { get { return LogManager.Logger; } }
        public IApplication Application { set { _application = value; } }
        public static ApConfig ApConfig { get; set; }
        public Guid Id { get { return PluginId; } }
        public string Name { get { return Resources.Plugin_Name; } }
        public string Version { get { return GetVersion(); } }
        public static IApplication GetApplication() { return _application; }
        public static string GetVersion() { return Assembly.GetExecutingAssembly().GetName().Version.ToString(3); }

        public static ApProxy GetProxy() {
            if (!_initialized)
                throw new ApplicationException("Plugin was not properly initialized. Unable to proceed.");

            if (string.IsNullOrEmpty(ApConfig.Profile.Username) || string.IsNullOrEmpty(ApConfig.Profile.Password))
                throw new ApplicationException(Resources.Error_ApCredentialsNotSpecified);

            if (ApConfig.Profile.CredentialsChanged ||
                (Proxy == null || Proxy.Expired ||
                Proxy.Username != ApConfig.Profile.Username ||
                Proxy.Password != ApConfig.Profile.Password)) {

                Proxy = ApProxy.Connect(BasePath, ApConfig.Profile.Username, ApConfig.Profile.Password);
            }

            return Proxy;
        }

        public void ReadOptions(XmlDocument xmlDoc, XmlNamespaceManager nsmgr, XmlElement pluginNode)
        {
            if (!_initialized) return;

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
                    ApConfig = new ApConfig(BasePath);
                }
                else {
                    if (ApConfig.Profile == null) {
                        ApConfig.Profile = new ApProfile(BasePath);
                    }
                    else {
                        ApConfig.Profile.ReloadConstantData(BasePath);
                    }

                    if (ApConfig.Mapping == null) {
                        ApConfig.Mapping = new ApMapping();
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Unable to read AttackPoint config:" + Environment.NewLine + ex.Message);
                Logger.LogMessage("Unable to read AttackPoint config.", ex);
            }
        }

        public void WriteOptions(XmlDocument xmlDoc, XmlElement pluginNode)
        {
            if (!_initialized) return;

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
                MessageBox.Show("Unable to save AttackPoint config:" + Environment.NewLine + ex.Message);
                Logger.LogMessage("Unable to save AttackPoint config.", ex);
            }
        }

        internal static List<StIntensity> UpdateStIntensities() {
            if (ApConfig.Mapping.Intensities == null || ApConfig.Mapping.Intensities.Count == 0) {
                ApConfig.Mapping.Intensities = new List<StIntensity>();
                for (int i = 0; i <= 10; ++i) {
                    ApConfig.Mapping.Intensities.Add(
                        new StIntensity() { StId = i.ToString() });
                }
            }

            return ApConfig.Mapping.Intensities;
        }

        internal static List<StHeartZoneCategory> UpdateStHeartRateZones() {
            var hrCategories = _application.Logbook.HeartRateZones;
            List<StHeartZoneCategory> list = null;

            if (ApConfig.Mapping.HeartZoneCatogories == null) {
                ApConfig.Mapping.HeartZoneCatogories = new List<StHeartZoneCategory>();
            }
            else {
                list = new List<StHeartZoneCategory>(ApConfig.Mapping.HeartZoneCatogories);
            }

            ApConfig.Mapping.HeartZoneCatogories.Clear();
            foreach (var cat in hrCategories) {
                StHeartZoneCategory c = null;
                if (list == null ||
                    (c = list.Find(t => t.Id == cat.ReferenceId)) == null) {
                    c = new StHeartZoneCategory(cat);
                }
                else {
                    c.Instance = cat;
                }

                ApConfig.Mapping.HeartZoneCatogories.Add(c);

                // Deal with zones in this category
                List<StHeartZone> zones = null;
                if (c.HeartZones == null) {
                    c.HeartZones = new List<StHeartZone>();
                }
                else {
                    zones = new List<StHeartZone>(c.HeartZones);
                }

                c.HeartZones.Clear();
                foreach (var zone in cat.Zones) {
                    StHeartZone z = null;
                    if (zones == null ||
                        ((z = zones.Find(t => t.StId == zone.Name)) == null)) {
                        z = new StHeartZone(zone);
                    }
                    else {
                        z.Instance = zone;
                    }
                    c.HeartZones.Add(z);
                }
            }

            return ApConfig.Mapping.HeartZoneCatogories;
        }

        internal static List<StEquipment> UpdateStEquipmentItems() {
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
                    e.Instance = item;
                }

                ApConfig.Mapping.Shoes.Add(e);
            }
            return ApConfig.Mapping.Shoes;
        }

        internal static List<StCategory> UpdateStCategories() {
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
            if (!Logger.IsDebug) return;
            if (data == null) return;
            if (data.IsEmpty()) {
                Logger.PrintMessage("ApActivity data is empty.");
                return;
            }
            var sb = new StringBuilder();
            using (var w = new StringWriter(sb)) {
                var ser = new XmlSerializer(typeof(ApActivityData));
                ser.Serialize(w, data);
            }
            Logger.PrintMessage("ApActivityData:" + Environment.NewLine + sb);
        }

        internal static Units GetDistanceUnits() {
            var units = GetApplication().SystemPreferences.DistanceUnits;
            return units == Length.Units.Centimeter || units == Length.Units.Kilometer || units == Length.Units.Meter ? Units.Metric : Units.English;
        }

        internal static bool IsNotesFormatValid(string format) {
            // TODO: Implement
            return true;
        }

        internal static void ShowWebPage(string page) {
            try {
                var process = new Process();
                process.StartInfo.FileName = page;
                process.Start();
            }
            catch (Exception ex) {
                Logger.LogMessage("Failed to open a browser.", ex);
                MessageBox.Show("Unable to open your browser:\n" + ex.Message);
            }
        }

        internal static void OpenEmailClient(string subject) {
            try {
                var process = new Process();
                process.StartInfo.FileName = string.Format("mailto:{0}?subject={1}", HttpUtility.UrlEncode(ApPlugin.FeedbackEmail), HttpUtility.UrlEncode(subject));
                process.Start();
            }
            catch (Exception ex) {
                MessageBox.Show("Unable to open your email client:\n" + ex.Message);
            }
        }

        internal static void HandleUnhandledException(Exception ex) {
            Logger.LogMessage("Unable to refresh ActivityControl.", ex);
            MessageBox.Show("Oops. It looks like there is a bug in AttackPoint plugin.\nPlease contact the developer.\n" + ex);
        }
    }
}
