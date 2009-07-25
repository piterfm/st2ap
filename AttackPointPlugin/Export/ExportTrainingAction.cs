using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

using ZoneFiveSoftware.Common.Data;
using ZoneFiveSoftware.Common.Data.Fitness;
using ZoneFiveSoftware.Common.Data.GPS;
using ZoneFiveSoftware.Common.Visuals;
using ZoneFiveSoftware.Common.Visuals.Fitness;

using GK.AttackPoint;
using GK.SportTracks.AttackPoint;

namespace GK.SportTracks.AttackPoint.Export
{
    public class ExportTrainingAction : ExportNoteAction
    {
        public ExportTrainingAction(IActivity activity) : base(activity) {}

        protected override ApNote CreateNote() { return new ApTraining(); }

        public override ExportError? Populate(ApNote note, IActivity activity, ApActivityData data, ILogbook logbook, ApMetadata metadata, ApConfig config) {
            var error = base.Populate(note, activity, data, logbook, metadata, config);
            if (error != null)
                return error;

            var training = (ApTraining)note;

            var stCategory = config.Mapping.Activities.Find(c => c.StId == activity.Category.ReferenceId);
            if (stCategory == null) {
                return ExportError.CategoryNotFound;
            }

            var ai = ActivityInfoCache.Instance.GetInfo(activity);
            if (ai.Time == TimeSpan.MinValue || ai.Time == TimeSpan.Zero) {
                return ExportError.TimeNotSpecified;
            }

            if (!config.Profile.Activities.Exists(a => a.Id == stCategory.ApId)) {
                return ExportError.CategoryNotMapped;
            }

            training.ActivityId = stCategory.ApId;

            if (!string.IsNullOrEmpty(data.ActivitySubtype)) {
                training.ActivitySubType = data.ActivitySubtype;
            }
            else if (!string.IsNullOrEmpty(stCategory.SubType)) {
                training.ActivitySubType = stCategory.SubType;
            }

            if (!string.IsNullOrEmpty(data.WorkoutId)) {
                training.WorkoutId = data.WorkoutId;
            }
            else if (!string.IsNullOrEmpty(stCategory.WorkoutId)) {
                training.WorkoutId = stCategory.WorkoutId;
            }

            var stIntensity = config.Mapping.Intensities.Find(i => i.StId == activity.Intensity.ToString());
            if (stIntensity == null) {
                return ExportError.IntensityNotFound;
            }

            var iMappingExists = config.Profile.Intensities.Exists(i => i.Id == stIntensity.ApId);
            // For advanced accounts we send mixed intensity if it is specified
            // Otherwise, we throw either the NotMapped exception or send intensity ID
            if (config.Profile.AdvancedFeaturesEnabled && data.IsMixedIntensitySpecified()) {
                for (int i = 0; i <= 5; ++i) {
                    training.Intensities[i] = data.Intensities[i];
                }
                training.IntensityId = "-1"; // Mixed intensity
            }
            else if (!iMappingExists) {
                return ExportError.IntensityNotMapped;
            }
            else {
                training.IntensityId = stIntensity.ApId;
            }

            training.Time = ai.Time;

            if (HasValue(ai.DistanceMeters)) {
                training.Distance = Math.Round(ai.DistanceMeters / 1000, 2).ToString();
                training.DistanceUnitId = metadata.GetUnitsValue(Quantity.Distance.ToString(), Units.Metric.ToString());
            }
            //else if (!string.IsNullOrEmpty(data.Pace)) {
            //    training.Pace = ApActivityData.ConvertPace(data.Pace, data.DistanceUnits, Units.Metric);
            //}
            else {
                return ExportError.DistanceNotSpecified;
            }

            float climb = float.NaN;
            if (HasValue(activity.TotalAscendMetersEntered)) {
                climb = activity.TotalAscendMetersEntered;
            }
            else {
                // TODO: Fix this
                if (logbook.ClimbZones != null) {
                    foreach (IZoneCategory zc in logbook.ClimbZones) {
                        climb = (float)ai.TotalAscendingMeters(zc);
                    }
                }
            }

            if (HasValue(climb)) {
                training.Climb = ConvertToString(climb, 1);
                training.ClimbUnitId = metadata.GetUnitsValue(Quantity.Climb.ToString(), Units.Metric.ToString());
            }

            ExportError? exportError = null;
            foreach (IEquipmentItem ei in activity.EquipmentUsed) {
                var stShoes = config.Mapping.Shoes.Find(shoe => shoe.StId == ei.ReferenceId);
                if (stShoes == null) {
                    return ExportError.EquipmentNotFound;
                }

                if (!config.Profile.Shoes.Exists(shoes => shoes.Id == stShoes.ApId)) {
                    exportError = ExportError.EquipmentNotMapped;
                }
                else {
                    training.ShoesId = stShoes.ApId;
                }

                // Pick the first one
                break;
            }

            training.AverageHeartRate = ConvertToString(ai.AverageHeartRate, 0);
            training.MaxHeartRate = ConvertToString(ai.MaximumHeartRate, 0);

            if (!string.IsNullOrEmpty(data.SpikedControls)) {
                training.SpikedControls = data.SpikedControls;
            }
            if (!string.IsNullOrEmpty(data.TotalControls)) {
                training.TotalControls = data.TotalControls;
            }
            if (!string.IsNullOrEmpty(data.TechnicalIntensityId)) {
                training.TechnicalIntensityId = data.TechnicalIntensityId;
            }

            return exportError;
        }

        public override string Title { get { return "AttackPoint training"; } }

    }
}
