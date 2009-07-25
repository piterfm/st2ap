using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace GK.AttackPoint
{
    public class ApActivity : ApEntity
    {
        [JsonProperty("graphorder")]
        [XmlAttribute]
        public string GraphOrder { get; set; }

        [JsonProperty("color")]
        [XmlAttribute]
        public string Color { get; set; }
    }
}
