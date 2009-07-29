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
    public class Test_ExportNote : BaseTest_ExportAction
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

            Assert.Equal(null, error);
            Assert.Equal(date, note.Date);
            Assert.Equal("2009", note.Year);
            Assert.Equal("07", note.MonthId);
            Assert.Equal("11", note.DayId);
            Assert.Equal("This is a note", note.Description);
            Assert.Equal("on", note.IsSick);
            Assert.Equal("on", note.IsInjured);
            Assert.Equal(null, note.IsRestDay);
            Assert.Equal("55", note.RestingHeartRate);
            Assert.Equal("64", note.Weight);
            Assert.Equal("kg", note.WeightUnitId);
            Assert.Equal("8", note.SleepHours);
            Assert.Equal(null, note.PrivateDescription);
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

            Assert.Equal(null, error);
            Assert.Equal(date, note.Date);
            Assert.Equal("2009", note.Year);
            Assert.Equal("07", note.MonthId);
            Assert.Equal("11", note.DayId);
            Assert.Equal(null, note.Description);
            Assert.Equal(null, note.IsSick);
            Assert.Equal(null, note.IsInjured);
            Assert.Equal(null, note.IsRestDay);
            Assert.Equal(null, note.RestingHeartRate);
            Assert.Equal(null, note.Weight);
            Assert.Equal(null, note.WeightUnitId);
            Assert.Equal(null, note.SleepHours);
            Assert.Equal(null, note.PrivateDescription);
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

            Assert.Equal(null, error);
            Assert.Equal(date, note.Date);

            // Check conversion from UTC to local time zone
            DateTime utcDate = date.ToUniversalTime();
            activity.SetupGet(a => a.StartTime).Returns(utcDate);
            error = action.Populate(note, activity.Object, edata);
            Assert.Equal(null, error);
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

            Assert.Equal(null, error);
            Assert.Equal(date, note.Date);
            Assert.Equal("2009", note.Year);
            Assert.Equal("07", note.MonthId);
            Assert.Equal("11", note.DayId);
            Assert.Equal("This is a note", note.Description);
            Assert.Equal(null, note.IsSick);
            Assert.Equal(null, note.IsInjured);
            Assert.Equal(null, note.IsRestDay);
            Assert.Equal(null, note.RestingHeartRate);
            Assert.Equal(null, note.Weight);
            Assert.Equal(null, note.WeightUnitId);
            Assert.Equal(null, note.SleepHours);
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

            Assert.Equal(null, error);
            Assert.Equal(date, note.Date);
            Assert.Equal("2009", note.Year);
            Assert.Equal("07", note.MonthId);
            Assert.Equal("11", note.DayId);
            Assert.Equal("This is a note", note.Description);
            Assert.Equal("on", note.IsSick);
            Assert.Equal("on", note.IsInjured);
            Assert.Equal(null, note.IsRestDay);
            Assert.Equal(null, note.RestingHeartRate);
            Assert.Equal(null, note.Weight);
            Assert.Equal("kg", note.WeightUnitId);
            Assert.Equal(null, note.SleepHours);
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

            Assert.Equal(null, error);
            Assert.Equal(date, note.Date);
            Assert.Equal("2009", note.Year);
            Assert.Equal("07", note.MonthId);
            Assert.Equal("11", note.DayId);
            Assert.Equal(null, note.Description);
            Assert.Equal(null, note.IsSick);
            Assert.Equal(null, note.IsInjured);
            Assert.Equal(null, note.IsRestDay);
            Assert.Equal(null, note.RestingHeartRate);
            Assert.Equal(null, note.Weight);
            Assert.Equal(null, note.WeightUnitId);
            Assert.Equal(null, note.SleepHours);
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

            Assert.Equal(null, error);
            Assert.Equal(date, note.Date);
            Assert.Equal("2009", note.Year);
            Assert.Equal("07", note.MonthId);
            Assert.Equal("11", note.DayId);
            Assert.Equal(null, note.Description);
            Assert.Equal(null, note.IsSick);
            Assert.Equal(null, note.IsInjured);
            Assert.Equal(null, note.IsRestDay);
            Assert.Equal(null, note.RestingHeartRate);
            Assert.Equal(null, note.Weight);
            Assert.Equal(null, note.WeightUnitId);
            Assert.Equal(null, note.SleepHours);
            Assert.Equal(null, note.PrivateDescription);
        }


    }
}
