using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using GK.AttackPoint;

namespace GK.SportTracks.AttackPoint
{
    [XmlRoot(ElementName = "AttackPointConfiguration", Namespace = ApConstantData.Namespace)] 
    public class ApConfig
    {
        public const string DefaultGpsTrackVisibility = "0";
        public const string NoUploadGpsTrackVisibility = "5";

        public ApConfig() { }
        public ApConfig(string basePath) {
            Profile = new ApProfile(basePath);
            Mapping = new ApMapping();
        }

        [XmlAttribute]
        public string NotesFormat { get; set; }
        [XmlAttribute]
        public string PrivateNotesFormat { get; set; }
        [XmlAttribute]
        public bool WarnOnNotMappedEquipment { get; set; }
        [XmlAttribute]
        public bool WarnOnUnspecifiedIntensity { get; set; }
        [XmlAttribute]
        public bool AutoCalculateMixedIntensity { get; set; }
        [XmlAttribute]
        public string GpsTrackVisibility { get; set; }

        public ApProfile Profile { get; set; }
        public ApMapping Mapping { get; set; }

        [XmlIgnore]
        public bool IsMappingEmpty {
            get {
                return
                    Mapping == null ||
                    Mapping.Activities == null ||
                    Mapping.Activities.Count == 0 ||
                    Mapping.Intensities == null ||
                    Mapping.Intensities.Count == 0 ||
                    Mapping.Shoes == null;/* ||
                    Mapping.Shoes.Count == 0;*/ // ST user may not have equipment at all
            }
        }

        public void Clear() {
            NotesFormat = null;
            PrivateNotesFormat = null;
            Profile.AdvancedFeaturesEnabledSpecified = false;
            Profile.Activities = null;
            Profile.Shoes = null;
            Mapping = new ApMapping();
        }

        public string GetGpsTrackVisibility() {
            return string.IsNullOrEmpty(GpsTrackVisibility) ? DefaultGpsTrackVisibility : GpsTrackVisibility;
        }
    }
}
