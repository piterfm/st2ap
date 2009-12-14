using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace GK.AttackPoint
{
    public class ApTraining : ApNote
    {
        private string[] _intensities = new string[6];

        public string ActivityId { get; set; }
        public string ActivitySubType { get; set; }
        public string WorkoutId { get; set; }
        public string IntensityId { get; set; }
        public TimeSpan Time { get; set; }
        public string Distance { get; set; }
        public string Pace { get; set; }
        public string DistanceUnitId { get; set; }
        public string Climb { get; set; }
        public string ClimbUnitId { get; set; }
        public string ShoesId { get; set; }
        public string AverageHeartRate { get; set; }
        public string MaxHeartRate { get; set; }
        public string SpikedControls { get; set; }
        public string TotalControls { get; set; }
        public string TechnicalIntensityId { get; set; }
        public string IsPlan { get; set; }
        public string SessionData { get; set; }
        public string IsPrivateLocation { get; set; }

        public string[] Intensities {
            get { return _intensities; }
            set { _intensities = value; }
        }
        public string Intensity0 {
            get { return _intensities[0]; }
            set { _intensities[0] = value; }
        }
        public string Intensity1 {
            get { return _intensities[1]; }
            set { _intensities[1] = value; }
        }
        public string Intensity2 {
            get { return _intensities[2]; }
            set { _intensities[2] = value; }
        }
        public string Intensity3 {
            get { return _intensities[3]; }
            set { _intensities[3] = value; }
        }
        public string Intensity4 {
            get { return _intensities[4]; }
            set { _intensities[4] = value; }
        }
        public string Intensity5 {
            get { return _intensities[5]; }
            set { _intensities[5] = value; }
        }

        public string TotalTime {
            get {
                if (Time != TimeSpan.Zero && Time != TimeSpan.MinValue) {
                    return string.Format("{0:D2}{1:D2}{2:D2}", Time.Hours, Time.Minutes, Time.Seconds);
                }
                return null;
            }
        }

    }
}
