using System;
using System.Collections.Generic;
using System.Text;
using GK.AttackPoint;
using ZoneFiveSoftware.Common.Data.Fitness;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace GK.SportTracks.AttackPoint.Export
{
    public class ExportNoteAction : ExportAction
    {
        private const string CourseSpecFormat = "Course: {0} {1} {2}";
        private static Regex FormatRegex = new Regex("\\[(.*?){(?<f>.*?)}(.*?)\\]", RegexOptions.Singleline);

        public ExportNoteAction(IActivity activity) : base(activity) { }

        protected override ApNote CreateNote() { return new ApNote(); }

        public override ExportError? Populate(ApNote note, IActivity activity, ExportConfig edata) {
            if (activity.StartTime == DateTime.MinValue || activity.StartTime == DateTime.MaxValue) {
                return ExportError.DateNotSpecified;
            }

            note.Date = activity.StartTime.Kind == DateTimeKind.Utc ?
                TimeZone.CurrentTimeZone.ToLocalTime(activity.StartTime) : activity.StartTime;

            var entry = edata.Logbook.Athlete.InfoEntries != null ? edata.Logbook.Athlete.InfoEntries.EntryForDate(activity.StartTime) : null;
            if (entry != null) {
                note.IsInjured = ConvertToString(entry.Injured);
                note.IsSick = ConvertToString(entry.Sick);
                note.RestingHeartRate = ConvertToString(entry.RestingHeartRatePerMinute);
                note.SleepHours = ConvertToString(entry.SleepHours);
                note.Weight = ConvertToString(entry.WeightKilograms);
                note.WeightUnitId = edata.Metadata.GetUnitsValue(Quantity.Weight.ToString(), Units.Metric.ToString());
            }

            //var ainfo = ActivityInfoCache.Instance.GetInfo(activity);

            var fields = new Dictionary<string, string>();
            AddField(fields, "Name", activity.Name);
            AddField(fields, "Location", activity.Location);
            AddField(fields, "Calories", activity.TotalCalories);
            AddField(fields, "Notes", activity.Notes);
            if (edata.ActivityData != null && (!string.IsNullOrEmpty(edata.ActivityData.CourseName) || !string.IsNullOrEmpty(edata.ActivityData.CourseLength))) {
                AddField(fields, "CourseSpec", string.Format(CourseSpecFormat,
                    (string.IsNullOrEmpty(edata.ActivityData.CourseName) ? string.Empty : edata.ActivityData.CourseName),
                    (string.IsNullOrEmpty(edata.ActivityData.CourseLength) ? string.Empty : edata.ActivityData.CourseLength + " km"),
                    (string.IsNullOrEmpty(edata.ActivityData.CourseClimb) ? string.Empty : edata.ActivityData.CourseClimb + " m"))
                    );
            }

            var notes = activity.Notes;
            if (!string.IsNullOrEmpty(edata.Config.NotesFormat)) {
                if (ApPlugin.IsNotesFormatValid(edata.Config.NotesFormat)) {
                    notes = FormatNotes(edata.Config.NotesFormat, fields, notes);
                }
                else {
                    edata.Warnings |= ExportWarning.FormatNotesFailed;
                }
            }

            note.Description = notes;

            if (edata.Config.Profile.AdvancedFeaturesEnabled && (edata.ActivityData != null)) {
                notes = edata.ActivityData.PrivateNote;
                if (!string.IsNullOrEmpty(edata.Config.PrivateNotesFormat)) {
                    if (ApPlugin.IsNotesFormatValid(edata.Config.PrivateNotesFormat)) {
                        notes = FormatNotes(edata.Config.PrivateNotesFormat, fields, notes);
                    }
                    else {
                        edata.Warnings |= ExportWarning.FormatNotesFailed;
                    }
                }
                note.PrivateDescription = notes;
            }

            return null;
        }

        public static string FormatNotes(string format, Dictionary<string, string> fields, string fallbackValue) {
            var result = format;
            try {
                var matches = FormatRegex.Matches(format);
                foreach (Match m in matches) {
                    var substitute = string.Empty;
                    var fieldName = m.Groups["f"].Value;
                    if (fields.ContainsKey(fieldName)) {
                        substitute = m.Value.Replace("{" + fieldName + "}", fields[fieldName]);
                        substitute = substitute.Substring(1, substitute.Length - 2);
                    }

                    result = result.Replace(m.Value, substitute);
                }
            }
            catch (Exception ex) {
                ApPlugin.Logger.LogMessage(string.Format("Unable to format a note.{0}Format string: {1}.",
                    Environment.NewLine, format) , ex);
                result = fallbackValue;
            }
            return result;
        }

        private void AddField(Dictionary<string, string> fields, string name, float value) {
            if (HasValue(value)) {
                fields.Add(name, value.ToString());
            }
        }

        private void AddField(Dictionary<string, string> fields, string name, string value) {
            if (!string.IsNullOrEmpty(value)) {
                fields.Add(name, value);
            }
        }

        public override string Title { get { return "AttackPoint note"; } }
    }
}
