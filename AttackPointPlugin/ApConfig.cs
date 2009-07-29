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
        [XmlAttribute]
        public string NotesFormat { get; set; }
        [XmlAttribute]
        public string PrivateNotesFormat { get; set; }
        [XmlAttribute]
        public bool WarnOnNotMappedEquipment { get; set; }
        [XmlAttribute]
        public bool WarnOnUnspecifiedIntensity { get; set; }

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
                    Mapping.Shoes == null ||
                    Mapping.Shoes.Count == 0;
            }
        }
    }
}
