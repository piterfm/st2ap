using System;
using System.Collections.Generic;
using System.Text;
using ZoneFiveSoftware.Common.Data.Fitness;
using GK.AttackPoint;
using System.Xml.Serialization;

namespace GK.SportTracks.AttackPoint
{
    public class StCategory : StEntity
    {
        private IActivityCategory _instance;
        public StCategory() { }

        public StCategory(IActivityCategory category) {
            Instance = category;
            IsOrienteering = ToString().IndexOf("orienteering", StringComparison.OrdinalIgnoreCase) > -1;
        }

        [XmlIgnore]
        public IActivityCategory Instance {
            get { return _instance; }
            set {
                _instance = value;
                StId = _instance.ReferenceId;
            }
        }

        [XmlAttribute]
        public string SubType { get; set; }
        [XmlAttribute]
        public string WorkoutId { get; set; }
        [XmlAttribute("O")]
        public bool IsOrienteering { get; set; }

        public override string ToString() {
            return GetFullName(Instance);
        }

        public static string GetFullName(IActivityCategory category) {
            var title = new StringBuilder(category.Name);
            var c = category.Parent;
            while (c != null && c.Parent != null) {
                title.Insert(0, c.Name + " > ");
                c = c.Parent;
            }

            return title.ToString();
        }
    }
}
