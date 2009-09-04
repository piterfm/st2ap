using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using GK.AttackPoint;
using GK.SportTracks.AttackPoint.Export;
using Moq;
using ZoneFiveSoftware.Common.Data.Fitness;
using GK.SportTracks.AttackPoint;

namespace AttackPointPluginTests
{
    public class Test_ExportNote : TestBase_ExportAction
    {
        [Fact]
        public void ExportNoteWithoutDate() {

            var activity = new Mock<IActivity>();

            Mock<IAthlete> athlete;
            var logbook = SetUpLogbook(DateTime.MinValue, out athlete);

            var action = new ExportNoteAction(activity.Object);
            var note = new ApNote();
            var edata = new ExportConfig()
            {
                ActivityData = null,
                Logbook = logbook.Object,
                Metadata = _metadata,
                Config = _config
            };

            var error = action.Populate(note, activity.Object, edata);

            Assert.Equal(ExportError.DateNotSpecified, error);
        }

        [Fact]
        public void ExportNote() {
            var date = new DateTime(2009, 7, 11);

            var activity = new Mock<IActivity>();
            activity.SetupGet(a => a.StartTime).Returns(date);
            activity.SetupGet(a => a.Notes).Returns("This is a note");

            Mock<IAthlete> athlete;
            var logbook = SetUpLogbook(date, out athlete);

            var action = new ExportNoteAction(activity.Object);
            var note = new ApNote();
            var edata = new ExportConfig()
            {
                ActivityData = null,
                Logbook = logbook.Object,
                Metadata = _metadata,
                Config = _config
            };

            var error = action.Populate(note, activity.Object, edata);

            Assert.Null(error);
            Assert.Equal(date, note.Date);
            Assert.Equal("2009", note.Year);
            Assert.Equal("07", note.MonthId);
            Assert.Equal("11", note.DayId);
            Assert.Equal("This is a note", note.Description);
            Assert.Equal("on", note.IsSick);
            Assert.Equal("on", note.IsInjured);
            Assert.Null(note.IsRestDay);
            Assert.Equal("55", note.RestingHeartRate);
            Assert.Equal("64", note.Weight);
            Assert.Equal("kg", note.WeightUnitId);
            Assert.Equal("8", note.SleepHours);
            Assert.Null(note.PrivateDescription);
        }

        [Fact]
        public void ExportEmptyNote() {
            var date = new DateTime(2009, 7, 11);

            var activity = new Mock<IActivity>();
            activity.SetupGet(a => a.StartTime).Returns(date);

            Mock<IAthlete> athlete;
            var logbook = SetUpLogbook(DateTime.MinValue, out athlete);

            var action = new ExportNoteAction(activity.Object);
            var note = new ApNote();
            var edata = new ExportConfig()
            {
                ActivityData = null,
                Logbook = logbook.Object,
                Metadata = _metadata,
                Config = _config
            };

            var error = action.Populate(note, activity.Object, edata);

            Assert.Null(error);
            Assert.Equal(date, note.Date);
            Assert.Equal("2009", note.Year);
            Assert.Equal("07", note.MonthId);
            Assert.Equal("11", note.DayId);
            Assert.Null(note.Description);
            Assert.Null(note.IsSick);
            Assert.Null(note.IsInjured);
            Assert.Null(note.IsRestDay);
            Assert.Null(note.RestingHeartRate);
            Assert.Null(note.Weight);
            Assert.Null(note.WeightUnitId);
            Assert.Null(note.SleepHours);
            Assert.Null(note.PrivateDescription);
        }

        [Fact]
        public void ExportNoteLocalVsUtcDate() {
            var date = new DateTime(2009, 7, 11);

            var activity = new Mock<IActivity>();
            activity.SetupGet(a => a.StartTime).Returns(date);

            Mock<IAthlete> athlete;
            var logbook = SetUpLogbook(date, out athlete);

            var action = new ExportNoteAction(activity.Object);
            var note = new ApNote();
            var edata = new ExportConfig()
            {
                ActivityData = null,
                Logbook = logbook.Object,
                Metadata = _metadata,
                Config = _config
            };

            var error = action.Populate(note, activity.Object, edata);

            Assert.Null(error);
            Assert.Equal(date, note.Date);

            // Check conversion from UTC to local time zone
            DateTime utcDate = date.ToUniversalTime();
            activity.SetupGet(a => a.StartTime).Returns(utcDate);
            error = action.Populate(note, activity.Object, edata);
            Assert.Null(error);
            Assert.Equal(date, note.Date);
        }

        [Fact]
        public void ExportNoteWithEmptyAthleteInfo() {
            var date = new DateTime(2009, 7, 11);

            var activity = new Mock<IActivity>();
            activity.SetupGet(a => a.StartTime).Returns(date);
            activity.SetupGet(a => a.Notes).Returns("This is a note");

            Mock<IAthlete> athlete;
            var logbook = SetUpLogbook(DateTime.MinValue, out athlete);

            var action = new ExportNoteAction(activity.Object);
            var note = new ApNote();
            var edata = new ExportConfig()
            {
                ActivityData = null,
                Logbook = logbook.Object,
                Metadata = _metadata,
                Config = _config
            };

            var error = action.Populate(note, activity.Object, edata);

            Assert.Null(error);
            Assert.Equal(date, note.Date);
            Assert.Equal("2009", note.Year);
            Assert.Equal("07", note.MonthId);
            Assert.Equal("11", note.DayId);
            Assert.Equal("This is a note", note.Description);
            Assert.Null(note.IsSick);
            Assert.Null(note.IsInjured);
            Assert.Null(note.IsRestDay);
            Assert.Null(note.RestingHeartRate);
            Assert.Null(note.Weight);
            Assert.Null(note.WeightUnitId);
            Assert.Null(note.SleepHours);
        }

        [Fact]
        public void ExportNoteWithNanAthleteInfo() {
            var date = new DateTime(2009, 7, 11);

            var activity = new Mock<IActivity>();
            activity.SetupGet(a => a.StartTime).Returns(date);
            activity.SetupGet(a => a.Notes).Returns("This is a note");

            Mock<IAthlete> athlete;
            var logbook = SetUpLogbookWithNanAthlete(date, out athlete);

            var action = new ExportNoteAction(activity.Object);
            var note = new ApNote();
            var edata = new ExportConfig()
            {
                ActivityData = null,
                Logbook = logbook.Object,
                Metadata = _metadata,
                Config = _config
            };

            var error = action.Populate(note, activity.Object, edata);

            Assert.Null(error);
            Assert.Equal(date, note.Date);
            Assert.Equal("2009", note.Year);
            Assert.Equal("07", note.MonthId);
            Assert.Equal("11", note.DayId);
            Assert.Equal("This is a note", note.Description);
            Assert.Equal("on", note.IsSick);
            Assert.Equal("on", note.IsInjured);
            Assert.Null(note.IsRestDay);
            Assert.Null(note.RestingHeartRate);
            Assert.Null(note.Weight);
            Assert.Equal("kg", note.WeightUnitId);
            Assert.Null(note.SleepHours);
        }

        [Fact]
        public void ExportNoteWithPrivateDescriptionForAdvancedAccount() {
            var date = new DateTime(2009, 7, 11);

            var activity = new Mock<IActivity>();
            activity.SetupGet(a => a.StartTime).Returns(date);

            Mock<IAthlete> athlete;
            var logbook = SetUpLogbook(DateTime.MinValue, out athlete);

            var apActivityData = new ApActivityData();
            apActivityData.PrivateNote = "private note";
            _config.Profile.AdvancedFeaturesEnabled = true;

            var action = new ExportNoteAction(activity.Object);
            var note = new ApNote();
            var edata = new ExportConfig()
            {
                ActivityData = apActivityData,
                Logbook = logbook.Object,
                Metadata = _metadata,
                Config = _config
            };

            var error = action.Populate(note, activity.Object, edata);

            Assert.Null(error);
            Assert.Equal(date, note.Date);
            Assert.Equal("2009", note.Year);
            Assert.Equal("07", note.MonthId);
            Assert.Equal("11", note.DayId);
            Assert.Null(note.Description);
            Assert.Null(note.IsSick);
            Assert.Null(note.IsInjured);
            Assert.Null(note.IsRestDay);
            Assert.Null(note.RestingHeartRate);
            Assert.Null(note.Weight);
            Assert.Null(note.WeightUnitId);
            Assert.Null(note.SleepHours);
            Assert.Equal("private note", note.PrivateDescription);
        }

        [Fact]
        public void ExportNoteWithPrivateDescriptionForNotAdvancedAccount() {
            var date = new DateTime(2009, 7, 11);

            var activity = new Mock<IActivity>();
            activity.SetupGet(a => a.StartTime).Returns(date);

            Mock<IAthlete> athlete;
            var logbook = SetUpLogbook(DateTime.MinValue, out athlete);

            var apActivityData = new ApActivityData();
            apActivityData.PrivateNote = "private note";
            _config.Profile.AdvancedFeaturesEnabled = false;

            var action = new ExportNoteAction(activity.Object);
            var note = new ApNote();
            var edata = new ExportConfig()
            {
                ActivityData = apActivityData,
                Logbook = logbook.Object,
                Metadata = _metadata,
                Config = _config
            };

            var error = action.Populate(note, activity.Object, edata);

            Assert.Null(error);
            Assert.Equal(date, note.Date);
            Assert.Equal("2009", note.Year);
            Assert.Equal("07", note.MonthId);
            Assert.Equal("11", note.DayId);
            Assert.Null(note.Description);
            Assert.Null(note.IsSick);
            Assert.Null(note.IsInjured);
            Assert.Null(note.IsRestDay);
            Assert.Null(note.RestingHeartRate);
            Assert.Null(note.Weight);
            Assert.Null(note.WeightUnitId);
            Assert.Null(note.SleepHours);
            Assert.Null(note.PrivateDescription);
        }

        [Fact]
        public void ExportNoteWithCustomFormat() {
            var date = new DateTime(2009, 7, 11);

            var activity = new Mock<IActivity>();
            activity.SetupGet(a => a.StartTime).Returns(date);
            activity.SetupGet(a => a.Notes).Returns("This is a note");
            activity.SetupGet(a => a.Name).Returns("Evening run");
            activity.SetupGet(a => a.Location).Returns("Rancho San Antonio");

            var weather = new Mock<IWeatherInfo>();
            weather.SetupGet(w => w.Conditions).Returns(WeatherConditions.Type.LightRain);
            weather.SetupGet(w => w.ConditionsText).Returns("Drizzling");
            weather.SetupGet(w => w.TemperatureCelsius).Returns(17.5f);
            activity.SetupGet(a => a.Weather).Returns(weather.Object);

            Mock<IAthlete> athlete;
            var logbook = SetUpLogbook(date, out athlete);

            // Set up note format
            _config.NotesFormat = "[{Name}. ][In {Location}. ][Burned {Calories} calories. ]\r\n[{WeatherTempF}.] [{WeatherTempC}.] [{WeatherConditions}.] [{WeatherDescription}.]\r\n[{Notes}]";

            var action = new ExportNoteAction(activity.Object);
            var note = new ApNote();
            var edata = new ExportConfig() {
                ActivityData = null,
                Logbook = logbook.Object,
                Metadata = _metadata,
                Config = _config
            };

            var error = action.Populate(note, activity.Object, edata);

            Assert.Null(error);
            Assert.Equal("", note.Description);
        }


    }
}
