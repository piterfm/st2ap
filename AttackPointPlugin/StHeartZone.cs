using System;
using System.Collections.Generic;
using System.Text;
using ZoneFiveSoftware.Common.Data.Fitness;
using GK.AttackPoint;
using System.Xml.Serialization;

namespace GK.SportTracks.AttackPoint
{
    public class StHeartZoneCategory
    {
        [XmlAttribute("Id")]
        public string Id { get; set; }
        [XmlArray("HeartZones")]
        [XmlArrayItem("HeartZone")]
        public List<StHeartZone> HeartZones { get; set; }
    }


    public class StHeartZone : StEntity
    {
        private INamedLowHighZone _instance;

        public StHeartZone() { }

        public StHeartZone(INamedLowHighZone zone) {
            Instance = zone;
        }

        [XmlIgnore]
        public INamedLowHighZone Instance {
            get { return _instance; }
            set {
                _instance = value;
                StId = _instance.Name;
            }
        }

        public override string ToString() {
            return Instance.Name;
        }


    }
}
