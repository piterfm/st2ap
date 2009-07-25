using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace GK.AttackPoint
{
    [XmlRoot(ElementName = "ap-metadata", Namespace = "http://www.atackpoint.org/metadata/v0.1")]
    public class ApMetadata
    {
        [XmlAttribute("base-url")]
        public string BaseUrl { get; set; }
        [XmlAttribute("login-url")]
        public string LogingUrl { get; set; }
        [XmlAttribute("login-cookie")]
        public string LoginCookieName { get; set; }
        [XmlAttribute("username-property")]
        public string UsernameProperyName { get; set; }
        [XmlAttribute("password-property")]
        public string PasswordPropertyName { get; set; }
        [XmlAttribute("scrape-activities-url")]
        public string ScrapeActivitiesUrl { get; set; }
        //[XmlAttribute("scrape-units-url")]
        //public string ScrapeUnitsUrl { get; set; }
        [XmlAttribute("scrape-shoes-url")]
        public string ScrapeShoesUrl { get; set; }
        [XmlAttribute("scrape-user-settings-url")]
        public string ScrapeUserSettingsUrl { get; set; }

        [XmlArray("operations")]
        [XmlArrayItem("operation")]
        public List<ApOperation> Operations { get; set; }

        [XmlArray("units")]
        [XmlArrayItem("unit")]
        public List<Unit> Units { get; set; }

        public string GetUnitsValue(string quantity, string unitsType) {
            return Units.Find(u => u.For == quantity && u.Type == unitsType).Value;
        }

    }

    public class ApOperation
    {
        [XmlAttribute("class")]
        public string ClassName { get; set; }
        [XmlAttribute("url")]
        public string PageUrl { get; set; }

        [XmlArray("properties")]
        [XmlArrayItem("property")]
        public List<ApOperationPropery> Properties { get; set; }
    }

    public class ApOperationPropery
    {
        [XmlAttribute("key")]
        public string Key { get; set; }
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("unicode")]
        public string Unicode { get; set; }
        [XmlAttribute("null-value")]
        public string NullValue { get; set; }
    }

    public class Unit
    {
        [XmlAttribute("for")]
        public string For { get; set; }
        [XmlAttribute("type")]
        public string Type { get; set; }
        [XmlAttribute("ap-name")]
        public string ApName { get; set; }
        [XmlAttribute("value")]
        public string Value { get; set; }
    }
}
