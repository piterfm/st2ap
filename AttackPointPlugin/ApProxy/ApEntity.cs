using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace GK.AttackPoint
{

    public class ApEntity
    {
        public ApEntity() { }

        [JsonProperty("id")]
        [XmlAttribute]
        public string Id { get; set; }
        [XmlAttribute]
        [JsonProperty("name")]
        public string Title { get; set; }

        public override string ToString() {
            return string.Format("[{0}] {1}", Id, Title);
        }
    }
}
