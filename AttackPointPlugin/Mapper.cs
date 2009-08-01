using System;
using System.Collections.Generic;
using System.Text;
using ZoneFiveSoftware.Common.Data.Fitness;
using GK.AttackPoint;
using System.Diagnostics;

namespace GK.SportTracks.AttackPoint
{
    public static class Mapper
    {
        // Default intensity mapping
        private static Dictionary<string, string> IntensityMap = new Dictionary<string, string>();

        static Mapper() {
            // ST intensity - AP intensity
            IntensityMap.Add("0", "0");
            IntensityMap.Add("1", "1");
            IntensityMap.Add("2", "1");
            IntensityMap.Add("3", "2");
            IntensityMap.Add("4", "2");
            IntensityMap.Add("5", "3");
            IntensityMap.Add("6", "3");
            IntensityMap.Add("7", "4");
            IntensityMap.Add("8", "4");
            IntensityMap.Add("9", "5");
            IntensityMap.Add("10", "5");
        }

        public static ApEntity MapCategory(ApProfile data, StCategory category, bool guess) {
            if (data.Activities == null || data.Activities.Count == 0)
                return null;

            var activity = data.Activities.Find(a => a.Id == category.ApId);

            if (activity == null && guess) {
                var temp = data.Activities.ConvertAll(a => (ApEntity)a);
                activity = (ApActivity)FindClosest(temp, category.ToString().Replace(">", " "));
            }

            return activity;
        }

        public static ApEntity MapIntensity(ApProfile data, StIntensity intensity, bool guess) {
            if (data.Intensities == null || data.Intensities.Count == 0)
                return null;

            var apIntensity = data.Intensities.Find(i => i.Id == intensity.ApId);

            if (apIntensity == null && guess) {
                apIntensity = data.Intensities.Find(i => i.Id == IntensityMap[intensity.StId]);
            }

            return apIntensity;
        }

        public static ApEntity MapEquipment(ApProfile data, StEquipment item, bool guess) {
            if (data.Shoes == null || data.Shoes.Count == 0)
                return null;

            var apShoes = data.Shoes.Find(s => s.Id == item.ApId);

            if (apShoes == null && guess) {
                var temp = data.Shoes.ConvertAll(shoes => (ApEntity)shoes);
                apShoes = (ApShoes)FindClosest(temp, item.ToString().Replace("-", " "));
            }

            return apShoes;
        }


        private static ApEntity FindClosest(List<ApEntity> list, string s) {
            int index = 0;
            int distance = 0;
            string[] words = s.Split(' ');
            for (int i = 0; i < list.Count; ++i) {
                int temp = 0;
                for (int k = 0; k < words.Length; ++k) {
                    if (words[k].Trim() == string.Empty)
                        continue;

                    if (list[i].Title.IndexOf(words[k], StringComparison.OrdinalIgnoreCase) > -1)
                        ++temp;
                }


                if (temp > distance) {
                    distance = temp;
                    index = i;
                }
            }

            return distance == 0 ? null : list[index];
        }


        internal static ApEntity MapHeartRateZone(ApProfile data, StHeartZone zone, bool guess) {
            if (data.Intensities == null || data.Intensities.Count == 0)
                return null;

            var apIntensity = data.Intensities.Find(i => i.Id == zone.ApId);

            // TODO: Implement guess
            //if (apIntensity == null && guess) {
            //    apIntensity = data.Intensities.Find(i => i.Id == IntensityMap[intensity.StId]);
            //}

            return apIntensity;
        }
    }

}
