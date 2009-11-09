using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using GK.Utils;

namespace GK.AttackPoint
{
    [XmlInclude(typeof(ApTraining))]
    public class ApNote
    {
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string PrivateDescription { get; set; }
        public string RestingHeartRate { get; set; }
        public string SleepHours { get; set; }
        public string IsInjured { get; set; }
        public string IsSick { get; set; }
        public string IsRestDay { get; set; }
        public string Weight { get; set; }
        public string WeightUnitId { get; set; }

        public string MonthId { get { return Date == DateTime.MinValue ? null : Format(Date.Month); } }
        public string DayId { get { return Date == DateTime.MinValue ? null : Format(Date.Day); } }
        public string Year { get { return Date == DateTime.MinValue ? null : Date.Year.ToString(); } }

        private string Format(int d) {
            return string.Format("{0:D2}", d);
        }

        public Dictionary<string, string> Pack(ApOperation operation) {
            var type = GetType();
            var parameters = new Dictionary<string, string>();
            foreach (var p in operation.Properties) {
                var pi = type.GetProperty(p.Name);
                object o = pi.GetValue(this, null);
                var value = o == null ? p.NullValue : o.ToString();
                if (value != null) {
                    // The server supports UTF-8 now instead of ISO-Latin
                    //parameters.Add(p.Key, p.Unicode == "true" ? EncodingUtils.ConvertForLatin1Html(value) : value);
                    parameters.Add(p.Key, value);
                };
            }

            return parameters;
        }
    }
}
