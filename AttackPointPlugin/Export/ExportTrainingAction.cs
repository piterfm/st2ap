using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using ZoneFiveSoftware.Common.Data;
using ZoneFiveSoftware.Common.Data.Fitness;
using ZoneFiveSoftware.Common.Data.GPS;
using ZoneFiveSoftware.Common.Visuals;
using ZoneFiveSoftware.Common.Visuals.Fitness;

using GK.AttackPoint;
using GK.SportTracks.AttackPoint;
using System.Collections.Generic;

namespace GK.SportTracks.AttackPoint.Export
{
    public class ExportTrainingAction : ExportNoteAction
    {
        public ExportTrainingAction(IActivity activity) : base(activity) { }
        public ExportTrainingAction(IList<IActivity> activities) : base(activities) { }

        protected override ApNote CreateNote() { return new ApTraining(); }

        public override ExportError? Populate(ApNote note, IActivity activity, ExportConfig edata) {
            var error = base.Populate(note, activity, edata);
            if (error != null)
                return error;

            var training = (ApTraining)note;

            var stCategory = edata.Config.Mapping.Activities.Find(c => c.StId == activity.Category.ReferenceId);
            if (stCategory == null) {
                return ExportError.CategoryNotFound;
            }

            var ai = ActivityInfoCache.Instance.GetInfo(activity);
            if (ai.Time == TimeSpan.MinValue || ai.Time == TimeSpan.Zero) {
                return ExportError.TimeNotSpecified;
            }

            if (!edata.Config.Profile.Activities.Exists(a => a.Id == stCategory.ApId)) {
                return ExportError.CategoryNotMapped;
            }

            training.ActivityId = stCategory.ApId;

            if (!string.IsNullOrEmpty(edata.ActivityData.ActivitySubtype)) {
                training.ActivitySubType = edata.ActivityData.ActivitySubtype;
            }
            else if (!string.IsNullOrEmpty(stCategory.SubType)) {
                training.ActivitySubType = stCategory.SubType;
            }

            if (!string.IsNullOrEmpty(edata.ActivityData.WorkoutId)) {
                training.WorkoutId = edata.ActivityData.WorkoutId;
            }
            else if (!string.IsNullOrEmpty(stCategory.WorkoutId)) {
                training.WorkoutId = stCategory.WorkoutId;
            }

            var stIntensity = edata.Config.Mapping.Intensities.Find(i => string.CompareOrdinal(i.StId, ConvertToString(activity.Intensity)) == 0);
            if (stIntensity == null) {
                return ExportError.IntensityNotFound;
            }

            var iMappingExists = edata.Config.Profile.Intensities.Exists(i => i.Id == stIntensity.ApId);
            // For advanced accounts we send mixed intensity if it is specified
            // Otherwise, we throw either the NotMapped exception or send intensity ID
            if (edata.Config.Profile.AdvancedFeaturesEnabled && edata.ActivityData.IsMixedIntensitySpecified()) {
                for (int i = 0; i <= 5; ++i) {
                    training.Intensities[i] = edata.ActivityData.Intensities[i];
                }
                training.IntensityId = "-1"; // Mixed intensity
            }
            else if (!iMappingExists) {
                return ExportError.IntensityNotMapped;
            }
            else {
                if (activity.Intensity == 0) {
                    edata.Warnings |= ExportWarning.IntensityNotSpecified;
                    training.IntensityId = "3"; // default value
                }
                else {
                    training.IntensityId = stIntensity.ApId;
                }
            }

            training.Time = ai.Time;

            if (HasValue(ai.DistanceMeters)) {
                Units distanceUnits = GetDistanceUnits(edata);
                training.Distance = ConvertToString(distanceUnits == Units.Metric ?
                    Math.Round(ai.DistanceMeters / 1000, 2) :
                    Math.Round(ai.DistanceMeters * 0.000621371192, 2)
                );
                training.DistanceUnitId = edata.Metadata.GetUnitsValue(Quantity.Distance.ToString(), distanceUnits.ToString());
            }

            float climb = float.NaN;
            if (HasValue(activity.TotalAscendMetersEntered)) {
                climb = activity.TotalAscendMetersEntered;
            }
            else {
                // TODO: Figure out where climb zones are used
                // I use the first climb zone to calcualte climb for all activity categories
                if (edata.Logbook.ClimbZones != null) {
                    foreach (IZoneCategory zc in edata.Logbook.ClimbZones) {
                        climb = (float)ai.TotalAscendingMeters(zc);
                        break;
                    }
                }
            }

            if (HasValue(climb)) {
                training.Climb = ConvertToString(climb, 1);
                training.ClimbUnitId = edata.Metadata.GetUnitsValue(Quantity.Climb.ToString(), Units.Metric.ToString());
            }

            foreach (IEquipmentItem ei in activity.EquipmentUsed) {
                var stShoes = edata.Config.Mapping.Shoes.Find(shoe => shoe.StId == ei.ReferenceId);
                if (stShoes == null) {
                    return ExportError.EquipmentNotFound;
                }

                if (!edata.Config.Profile.Shoes.Exists(shoes => shoes.Id == stShoes.ApId)) {
                    edata.Warnings |= ExportWarning.EquipmentNotMapped;
                }
                else {
                    training.ShoesId = stShoes.ApId;
                }

                // Pick the first one
                break;
            }

            training.AverageHeartRate = ConvertToString(ai.AverageHeartRate, 0);
            training.MaxHeartRate = ConvertToString(ai.MaximumHeartRate, 0);

            if (!string.IsNullOrEmpty(edata.ActivityData.SpikedControls)) {
                training.SpikedControls = edata.ActivityData.SpikedControls;
            }
            if (!string.IsNullOrEmpty(edata.ActivityData.TotalControls)) {
                training.TotalControls = edata.ActivityData.TotalControls;
            }
            if (!string.IsNullOrEmpty(edata.ActivityData.TechnicalIntensityId)) {
                training.TechnicalIntensityId = edata.ActivityData.TechnicalIntensityId;
            }

            if (edata.Config.Profile.AdvancedFeaturesEnabled) {
                training.GpsTrackVisibility = string.IsNullOrEmpty(edata.ActivityData.GpsTrackVisibility) ?
                    edata.Config.GetGpsTrackVisibility() : edata.ActivityData.GpsTrackVisibility;

                if (ai.HasAnyTrackData && !string.Equals(training.GpsTrackVisibility, ApConfig.NoUploadGpsTrackVisibility)) {
                    int sessionSeconds = (int)Math.Ceiling(ai.ActualTrackTime.TotalSeconds);
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("@samples");
                    int liveSeconds = 0;
                    int ri = 0;
                    int di = 0;
                    int hri = 0;
                    for (int elapsedSeconds = 0; elapsedSeconds <= sessionSeconds; elapsedSeconds++, liveSeconds++) {
                        DateTime actualTime = ai.ActualTrackStart.AddSeconds(elapsedSeconds);
                        string hrv = "";
                        string dmv = "";
                        string lat = "";
                        string lon = "";
                        string ele = "";
                        bool anydata = false;

                        if (activity.GPSRoute != null && ri < activity.GPSRoute.Count) {
                            ITimeValueEntry<IGPSPoint> rp = activity.GPSRoute[ri];
                            if (activity.GPSRoute.StartTime.AddSeconds(rp.ElapsedSeconds) == actualTime) {
                                lat = ConvertToString(rp.Value.LatitudeDegrees);
                                lon = ConvertToString(rp.Value.LongitudeDegrees);
                                ele = rp.Value.ElevationMeters.ToString("#.#", _formatProvider);
                                anydata = true;
                                ri++;
                            }
                        }
                        if (ai.HasDistanceData && di < ai.MovingDistanceMetersTrack.Count) {
                            ITimeValueEntry<float> dp = ai.MovingDistanceMetersTrack[di];
                            if (ai.MovingDistanceMetersTrack.StartTime.AddSeconds(dp.ElapsedSeconds) == actualTime) {
                                dmv = dp.Value.ToString("#.#", _formatProvider);
                                anydata = true;
                                di++;
                            }
                        }

                        if (activity.HeartRatePerMinuteTrack != null && hri < activity.HeartRatePerMinuteTrack.Count) {
                            ITimeValueEntry<float> hrp = activity.HeartRatePerMinuteTrack[hri];
                            if (activity.HeartRatePerMinuteTrack.StartTime.AddSeconds(hrp.ElapsedSeconds) == actualTime) {
                                hrv = hrp.Value.ToString("#", _formatProvider);
                                anydata = true;
                                hri++;
                            }
                        }
                        if (anydata) {
                            sb.AppendLine(String.Format("{0},{1},{2},{3},{4},{5},{6}", ConvertToString(liveSeconds), hrv, dmv, lat, lon, ele, ""));
                        }

                        if (activity.TimerPauses != null) {
                            for (int tpi = 0; tpi < activity.TimerPauses.Count; tpi++) {
                                IValueRange<DateTime> pp = activity.TimerPauses[tpi];
                                if (actualTime < pp.Lower) break;
                                if (pp.Lower <= actualTime && actualTime < pp.Upper) {
                                    // in a pause range
                                    liveSeconds--;
                                }
                            }
                        }

                    }
                    if (ai.RecordedLapDetailInfo.Count > 0) {
                        sb.AppendLine("@laps");
                        foreach (LapDetailInfo lap in ai.RecordedLapDetailInfo) {
                            sb.AppendLine(String.Format("{0},{1},{2},{3}", ConvertToString(lap.LapElapsed.TotalSeconds), ConvertToString(lap.LapDistanceMeters), ConvertToString(Math.Round(lap.AverageHeartRatePerMinute)), ConvertToString(Math.Round(lap.MaximumHeartRatePerMinute))));
                        }
                    }
                    training.SessionData = sb.ToString();
                }
            }
            
            return null;
        }

        public override string Title { get { return BatchMode ? "AttackPoint trainings" : "AttackPoint training"; } }

    }
}
