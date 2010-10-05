using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

using ZoneFiveSoftware.Common.Data.Fitness;
using ZoneFiveSoftware.Common.Data.Measurement;
using ZoneFiveSoftware.Common.Visuals.Fitness;

using GK.AttackPoint;
using GK.SportTracks.AttackPoint.Properties;

namespace GK.SportTracks.AttackPoint.Export
{
    public class ExportNoteAction : ExportAction
    {
        private const string CourseSpecFormat = "Course: {0} {1} {2}";
        private static Regex FormatRegex = new Regex("\\[(.*?){(?<f>.*?)}(.*?)\\]", RegexOptions.Singleline);

        public ExportNoteAction(IDailyActivityView view) : base(view) { }
        public ExportNoteAction(IActivityReportsView view) : base(view) { }

        protected override ApNote CreateNote() { return new ApNote(); }

        public override ExportError? Populate(ApNote note, IActivity activity, ExportConfig edata) {
            if (activity.StartTime == DateTime.MinValue || activity.StartTime == DateTime.MaxValue) {
                return ExportError.DateNotSpecified;
            }

            note.Date = AdjustDateTime(activity.StartTime);

            var entry = edata.Logbook.Athlete.InfoEntries != null ? edata.Logbook.Athlete.InfoEntries.EntryForDate(activity.StartTime) : null;
            if (entry != null) {
                note.IsInjured = ConvertToString(entry.Injured);
                note.IsSick = ConvertToString(entry.Sick);
                note.RestingHeartRate = ConvertToString(entry.RestingHeartRatePerMinute);
                note.SleepHours = ConvertToString(entry.SleepHours);
                note.Weight = ConvertToString(entry.WeightKilograms);
                note.WeightUnitId = edata.Metadata.GetUnitsValue(Quantity.Weight.ToString(), Units.Metric.ToString());
            }

            var fields = new Dictionary<string, string>();
            if (note.Date.TimeOfDay != TimeSpan.Zero) {
                AddField(fields, "TimeOfDay", note.Date.ToShortTimeString());
                AddField(fields, "PartOfDay", GetPartOfDay(note.Date));
            }
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

            if (activity.Weather != null) {
                AddField(fields, "WeatherTempF", ToFahrenheit(activity.Weather.TemperatureCelsius));
                AddField(fields, "WeatherTempC", activity.Weather.TemperatureCelsius != float.NaN ? (FormatTemperature(activity.Weather.TemperatureCelsius, 'C')) : string.Empty);
                AddField(fields, "WeatherConditions", Resources.ResourceManager.GetString("W_" + activity.Weather.Conditions));
                AddField(fields, "WeatherDescription", activity.Weather.ConditionsText);
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

            if (activity.HasStartTime) {
                note.SessionStartHour = ConvertToString(AdjustDateTime(FindSessionStart(activity, edata)).Hour);
            }

            return null;
        }

        protected DateTime FindSessionStart(IActivity activity, ExportConfig edata) {
            // logbook not sorted by date
            foreach (IActivity otherActivity in edata.Logbook.Activities) {
                if (otherActivity.HasStartTime) {
                    double startOffsetHrs = activity.StartTime.Subtract(otherActivity.StartTime).TotalHours;
                    if (startOffsetHrs > 0 && startOffsetHrs < 24) {
                        ActivityInfo aiOther = ActivityInfoCache.Instance.GetInfo(otherActivity);
                        double gapTimeHrs = activity.StartTime.Subtract(aiOther.ActualTrackEnd).TotalHours;
                        if (gapTimeHrs < 1.0) {
                            return FindSessionStart(otherActivity, edata);
                        }
                    }
                }
            }
            return activity.StartTime;
        }


        public static string GetPartOfDay(DateTime dateTime) {
            /*
             morning 4 - 10:59:59
             middday 11 - 13:59:59
             afternoon 14 - 17:29:59
             evening 17:30 - 20:59:59
             night 21 - 3:59:59
            */
            double hours = dateTime.TimeOfDay.TotalHours;
            if (hours >= 4 && hours < 11)
                return "Morning";
            else if (hours >= 11 && hours < 14)
                return "Midday";
            else if (hours >= 14 && hours < 17.5)
                return "Afternoon";
            else if (hours >= 17.5 && hours < 21)
                return "Evening";

            return "Night";
        }

        private string ToFahrenheit(float temperature) {
            return temperature != float.NaN ? (FormatTemperature(temperature * 9 / 5 + 32, 'F')) : string.Empty;
        }

        private string FormatTemperature(float temperature, char unit) {
            if (float.IsNaN(temperature)) return null;
            return string.Format("{0:N1} \u00b0{1}", temperature, unit);
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
                fields.Add(name, ConvertToString(value));
            }
        }

        private void AddField(Dictionary<string, string> fields, string name, string value) {
            if (!string.IsNullOrEmpty(value)) {
                fields.Add(name, value);
            }
        }

        internal static Units GetDistanceUnits(ExportConfig config) {
            var units =config.SystemPreferences.DistanceUnits;
            return units == Length.Units.Centimeter || units == Length.Units.Kilometer || units == Length.Units.Meter ? Units.Metric : Units.English;
        }

        public override string Title { get { return BatchMode ? "AttackPoint notes" : "AttackPoint note"; } }
    }
}
