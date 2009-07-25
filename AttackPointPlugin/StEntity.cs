using System;
using System.Collections.Generic;
using System.Text;
using ZoneFiveSoftware.Common.Data.Fitness;
using GK.AttackPoint;
using System.Xml.Serialization;

namespace GK.SportTracks.AttackPoint
{
    public class StEntity : IMappedEntity
    {
        [XmlAttribute]
        public string ApId { get; set; }
        [XmlAttribute]
        public string StId { get; set; }
    }
}
