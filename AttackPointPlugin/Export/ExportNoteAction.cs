using System;
using System.Collections.Generic;
using System.Text;
using GK.AttackPoint;
using ZoneFiveSoftware.Common.Data.Fitness;
using System.Windows.Forms;

namespace GK.SportTracks.AttackPoint.Export
{
    public class ExportNoteAction : ExportAction
    {
        public ExportNoteAction(IActivity activity) : base(activity) { }

        protected override ApNote CreateNote() { return new ApNote(); }

        public override ExportError? Populate(ApNote note, IActivity activity, ApActivityData data, ILogbook logbook, ApMetadata metadata, ApConfig config) {
            if (activity.StartTime == DateTime.MinValue || activity.StartTime == DateTime.MaxValue) {
                return ExportError.DateNotSpecified;
            }

            note.Date = activity.StartTime.Kind == DateTimeKind.Utc ?
                TimeZone.CurrentTimeZone.ToLocalTime(activity.StartTime) : activity.StartTime;

            var entry = logbook.Athlete.InfoEntries != null ? logbook.Athlete.InfoEntries.EntryForDate(activity.StartTime) : null;
            if (entry != null) {
                note.IsInjured = ConvertToString(entry.Injured);
                note.IsSick = ConvertToString(entry.Sick);
                note.RestingHeartRate = ConvertToString(entry.RestingHeartRatePerMinute);
                note.SleepHours = ConvertToString(entry.SleepHours);
                note.Weight = ConvertToString(entry.WeightKilograms);
                note.WeightUnitId = metadata.GetUnitsValue(Quantity.Weight.ToString(), Units.Metric.ToString());
            }

            var notes = activity.Notes;
            if (!string.IsNullOrEmpty(config.NotesFormat)) {
                notes = config.NotesFormat;
                notes = notes.Replace("[name]", ConvertToEmptyIfNull(activity.Name));
                notes = notes.Replace("[location]", ConvertToEmptyIfNull(activity.Location));
                notes = notes.Replace("[calories]", ConvertToEmptyIfNull(ConvertToString(activity.TotalCalories)));
                notes = notes.Replace("[description]", ConvertToEmptyIfNull(activity.Notes));
                if (!string.IsNullOrEmpty(data.CourseLength)) {
                    notes = notes.Replace("[course-spec]", string.Format("Course: {0} km, {1} m", data.CourseLength, data.CourseClimb));
                }
                else {
                    notes = notes.Replace("[course-spec]", string.Empty);
                }

                notes = notes.Replace("[notes]", activity.Notes);
            }

            note.Description = notes;

            if (config.Profile.AdvancedFeaturesEnabled && (data != null) && !string.IsNullOrEmpty(data.PrivateNote)) {
                note.PrivateDescription = data.PrivateNote;
            }

            return null;
        }

        public override string Title { get { return "AttackPoint note"; } }
    }
}
