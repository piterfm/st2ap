using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using GK.AttackPoint;
using System.Diagnostics;

namespace GK.SportTracks.AttackPoint
{
    /*****************************************************************/
    // AP support input of either distance or pace
    // Commented out Pace property. Only distance will be supported
    // It is unlikely anybody will need this functionality
    /*****************************************************************/

    [XmlRoot("a")]
    public class ApActivityData
    {
        private string[] _intensities = new string[6];

        [XmlAttribute("w")]
        public string WorkoutId { get; set; }
        [XmlAttribute("s")]
        public string SpikedControls { get; set; }
        [XmlAttribute("t")]
        public string TotalControls { get; set; }
        [XmlAttribute("cn")]
        public string CourseName { get; set; }
        [XmlAttribute("l")]
        public string CourseLength { get; set; }
        [XmlAttribute("c")]
        public string CourseClimb { get; set; }
        [XmlAttribute("i")]
        public string TechnicalIntensityId { get; set; }
        [XmlAttribute("u")]
        public string ActivitySubtype { get; set; }
        [XmlAttribute("n")]
        public string PrivateNote { get; set; }

        [XmlArray("ii")]
        [XmlArrayItem("i")]
        public string[] Intensities {
            get { return _intensities; }
            set { _intensities = value; }
        }

        public bool IsEmpty() {
            return
                (string.IsNullOrEmpty(WorkoutId) || (WorkoutId == "1")) &&
                string.IsNullOrEmpty(ActivitySubtype) &&
                !IsMixedIntensitySpecified() &&
                string.IsNullOrEmpty(SpikedControls) &&
                string.IsNullOrEmpty(TotalControls) &&
                (string.IsNullOrEmpty(TechnicalIntensityId) || (TechnicalIntensityId == "0")) &&
                string.IsNullOrEmpty(CourseName) &&
                string.IsNullOrEmpty(CourseLength) &&
                string.IsNullOrEmpty(CourseClimb) ?
                true : false;
        }

        public bool IsMixedIntensitySpecified() {
            for (int i = 0; i <= 5; ++i) {
                if (!string.IsNullOrEmpty(Intensities[i]))
                    return true;
            }
            return false;
        }

        public TimeSpan GetMixedIntensityTime() {
            var result = TimeSpan.Zero;
            for (int i = 0; i <= 5; ++i) {
                result += GetIntensityTime(Intensities[i]);
            }

            return result;
        }

        public static TimeSpan GetIntensityTime(string s) {
            // Format must be 'hhmmss' where all parts are optional

            if (string.IsNullOrEmpty(s) || s.Length > 6)
                return TimeSpan.Zero;

            try {
                var length = s.Length;
                if (length >= 1 && length <= 2)
                    return new TimeSpan(0, 0, int.Parse(s));

                if (length <= 4) {
                    if (length == 3) {
                        s = "0" + s;
                    }
                    return new TimeSpan(0, int.Parse(s.Substring(0, 2)), int.Parse(s.Substring(2)));
                }

                if (length < 6) {
                    s = "0" + s;
                }

                var ts = new TimeSpan(
                    int.Parse(s.Substring(0, 2)),
                    int.Parse(s.Substring(2, 2)),
                    int.Parse(s.Substring(4)));

                // We don't support times more than 24 hours
                if (ts.TotalHours >= 24F)
                    return TimeSpan.Zero;

                return ts;
            }
            catch (Exception ex) {
                ApPlugin.Logger.LogMessage("Unable to parse intensity time.", ex);
                return TimeSpan.Zero;
            }
        }

        public bool IsSingleIntensitySpecified(TimeSpan timeSpan, int intensity) {
            if (!IsMixedIntensitySpecified()) return false;

            for (int i = 0; i <= 5; ++i) {
                if (!string.IsNullOrEmpty(Intensities[i]) &&
                    (i != intensity || GetIntensityTime(Intensities[i]) != timeSpan))
                    return false;
            }
            return true;
        }
    }
}
