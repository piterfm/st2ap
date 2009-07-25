using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace GK.AttackPoint
{
    public class ApShoes : ApEntity
    {
        [JsonProperty("dateNew")]
        [XmlAttribute]
        public DateTime DateNew { get; set; }

        [JsonProperty("retired")]
        [XmlAttribute]
        public bool Retired { get; set; }

        [JsonProperty("lastUsed")]
        [XmlIgnore]
        public DateTime LastUsed { get; set; }

        [XmlIgnore]
        public string FullTitle {
            get { return string.Format("{0}{1}", Title, (Retired ? " (retired)" : string.Empty)); }
        }

        public override string ToString() {
            return string.Format("[{0}] {1}", Id, FullTitle);
        }

    }
}
