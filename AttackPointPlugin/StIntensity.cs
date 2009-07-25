using System;
using System.Collections.Generic;
using System.Text;
using ZoneFiveSoftware.Common.Data.Fitness;
using GK.AttackPoint;
using System.Xml.Serialization;

namespace GK.SportTracks.AttackPoint
{
    public class StIntensity : StEntity
    {
        private static Dictionary<string, string> Titles = new Dictionary<string, string>();

        static StIntensity() {
            Titles.Add("0", "None");
            Titles.Add("1", "Minimal");
            Titles.Add("2", "Minimal");
            Titles.Add("3", "Moderate");
            Titles.Add("4", "Moderate");
            Titles.Add("5", "Hard");
            Titles.Add("6", "Hard");
            Titles.Add("7", "Very hard");
            Titles.Add("8", "Very hard");
            Titles.Add("9", "Maximum");
            Titles.Add("10", "Maximum");
        }

        [XmlIgnore]
        public string Title {
            get {
                return Titles[StId];
            }
        }

        public override string ToString() {
            return StId + " - " + Title;
        }

    }
}
