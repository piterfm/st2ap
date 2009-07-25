using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace GK.AttackPoint
{
    [XmlRoot(ElementName = "ap-constant-data", Namespace = ApConstantData.Namespace)]
    public class ApConstantData
    {
        public const string Namespace = "http://www.attackpoint.org/config/v0.1";

        public List<ApEntity> Intensities { get; set; }
        public List<ApEntity> Workouts { get; set; }
        public List<ApEntity> TechnicalIntensities { get; set; }

    }
}
