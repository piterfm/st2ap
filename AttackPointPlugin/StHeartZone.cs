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
        private IZoneCategory _instance;

        public StHeartZoneCategory() { }
        public StHeartZoneCategory(IZoneCategory category) { Instance = category; }

        [XmlAttribute("Id")]
        public string Id { get; set; }
        [XmlArray("HeartZones")]
        [XmlArrayItem("HeartZone")]
        public List<StHeartZone> HeartZones { get; set; }
        [XmlIgnore]
        public string Title { get { return _instance == null ? "<Unknown>" : _instance.Name; } }

        [XmlIgnore]
        public IZoneCategory Instance {
            get { return _instance; }
            set {
                _instance = value;
                Id = _instance.ReferenceId;
            }
        }

        public override string ToString() {
            return Title;
        }
    }


    public class StHeartZone : StEntity
    {
        private INamedLowHighZone _instance;

        public StHeartZone() { }

        public StHeartZone(INamedLowHighZone zone) {
            Instance = zone;
        }

        [XmlIgnore]
        public string Title { get { return StId; } }

        [XmlIgnore]
        public INamedLowHighZone Instance {
            get { return _instance; }
            set {
                _instance = value;
                StId = _instance.Name;
            }
        }

        public override string ToString() {
            return Title;
        }


    }
}
