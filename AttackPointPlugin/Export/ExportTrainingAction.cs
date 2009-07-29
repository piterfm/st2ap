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

            var stIntensity = edata.Config.Mapping.Intensities.Find(i => i.StId == activity.Intensity.ToString());
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
                training.IntensityId = stIntensity.ApId;
                if (activity.Intensity == 0) {
                    edata.Warnings |= ExportWarning.IntensityNotSpecified;
                }
            }

            training.Time = ai.Time;

            if (HasValue(ai.DistanceMeters)) {
                training.Distance = Math.Round(ai.DistanceMeters / 1000, 2).ToString();
                training.DistanceUnitId = edata.Metadata.GetUnitsValue(Quantity.Distance.ToString(), Units.Metric.ToString());
            }
            else {
                return ExportError.DistanceNotSpecified;
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

            return null;
        }

        public override string Title { get { return "AttackPoint training"; } }

    }
}
