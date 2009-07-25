using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace GK.SportTracks.AttackPoint
{
    public class ApMapping
    {
        [XmlArrayItem("Activity")]
        public List<StCategory> Activities { get; set; }
        [XmlArrayItem("Intensity")]
        public List<StIntensity> Intensities { get; set; }
        [XmlArray("Shoez")]
        [XmlArrayItem("Shoes")]
        public List<StEquipment> Shoes { get; set; }
    }

}
