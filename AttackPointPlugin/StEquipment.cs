using System;
using System.Collections.Generic;
using System.Text;
using ZoneFiveSoftware.Common.Data.Fitness;
using GK.AttackPoint;
using System.Xml.Serialization;

namespace GK.SportTracks.AttackPoint
{
    public class StEquipment : StEntity
    {
        private IEquipmentItem _instance;
        public StEquipment() { }

        public StEquipment(IEquipmentItem category) {
            Instance = category;
        }

        [XmlIgnore]
        public IEquipmentItem Instance {
            get { return _instance; }
            set {
                _instance = value;
                StId = _instance.ReferenceId;
            }
        }

        public override string ToString() {
            return Instance.Name;
        }

    }
}
