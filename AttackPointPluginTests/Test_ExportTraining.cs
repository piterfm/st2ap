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
    public class Test_ExportTraining : BaseTest_ExportAction
    {
        [Fact]
        public void ExportTraining() {
            var date = new DateTime(2009, 7, 11);

            var data = new ApActivityData()
            {
                SpikedControls = "16",
                TotalControls = "20",
                TechnicalIntensityId = "4"
            };

            Mock<IAthlete> athlete;
            var logbook = SetUpLogbook(DateTime.MinValue, out athlete);

            var activity = new Mock<IActivity>();
            activity.SetupGet(a => a.StartTime).Returns(date);
            activity.SetupGet(a => a.Notes).Returns("This is a note");

            // Setup training
            var category = new Mock<IActivityCategory>();
            category.SetupGet(c => c.ReferenceId).Returns("5e13c6e5-59d5-4e99-9fbe-16c65e1111e1");
            activity.SetupGet(a => a.Category).Returns(category.Object);
            activity.SetupGet(a => a.TotalTimeEntered).Returns(new TimeSpan(1, 34, 21));
            activity.SetupGet(a => a.Intensity).Returns(8);
            activity.SetupGet(a => a.TotalDistanceMetersEntered).Returns(15.2F);
            activity.SetupGet(a => a.TotalAscendMetersEntered).Returns(460.5F);
            var shoes = new Mock<IEquipmentItem>();
            shoes.SetupGet(s => s.ReferenceId).Returns("783cc7b1-4a73-4b3f-b90f-7c96282406a3");
            var equipment = new List<IEquipmentItem>();
            equipment.Add(shoes.Object);
            activity.SetupGet(a => a.EquipmentUsed).Returns(equipment);
            activity.SetupGet(a => a.AverageHeartRatePerMinuteEntered).Returns(170F);
            activity.SetupGet(a => a.MaximumHeartRatePerMinuteEntered).Returns(186F);

            _config.Profile.AdvancedFeaturesEnabled = false;
            var action = new ExportTrainingAction(activity.Object);
            var note = new ApTraining();
            var edata = new ExportConfig()
            {
                ActivityData = data,
                Logbook = logbook.Object,
                Metadata = _metadata,
                Config = _config
            };

            var error = action.Populate(note, activity.Object, edata);

            Assert.Equal(null, error);
            AssertDate(note, date);
            AssertAthlete(note);
            Assert.Equal("This is a note", note.Description);

            // Assert training
            Assert.Equal("013421", note.TotalTime);
            Assert.Equal("4", note.IntensityId);
            Assert.Equal(null, note.Intensity0);
            Assert.Equal(null, note.Intensity1);
            Assert.Equal(null, note.Intensity2);
            Assert.Equal(null, note.Intensity3);
            Assert.Equal(null, note.Intensity4);
            Assert.Equal(null, note.Intensity5);
            Assert.Equal("5785", note.ActivityId);
            Assert.Equal("long", note.ActivitySubType);
            Assert.Equal("1", note.WorkoutId);
            Assert.Equal("2602", note.ShoesId);
            Assert.Equal("170", note.AverageHeartRate);
            Assert.Equal("186", note.MaxHeartRate);
            Assert.Equal("16", note.SpikedControls);
            Assert.Equal("20", note.TotalControls);
            Assert.Equal("4", note.TechnicalIntensityId);
        }

        [Fact]
        public void ExportTrainingWithMixedIntensity() {
            var date = new DateTime(2009, 7, 11);

            var data = new ApActivityData()
            {
                SpikedControls = "16",
                TotalControls = "20",
                TechnicalIntensityId = "4"
            };

            data.Intensities[0] = "10";
            data.Intensities[1] = "11";
            data.Intensities[2] = "12";
            data.Intensities[3] = "13";
            data.Intensities[4] = "14";
            data.Intensities[5] = "15";

            Mock<IAthlete> athlete;
            var logbook = SetUpLogbook(DateTime.MinValue, out athlete);

            var activity = new Mock<IActivity>();
            activity.SetupGet(a => a.StartTime).Returns(date);
            activity.SetupGet(a => a.Notes).Returns("This is a note");

            // Setup training
            var category = new Mock<IActivityCategory>();
            category.SetupGet(c => c.ReferenceId).Returns("5e13c6e5-59d5-4e99-9fbe-16c65e1111e1");
            activity.SetupGet(a => a.Category).Returns(category.Object);
            activity.SetupGet(a => a.TotalTimeEntered).Returns(new TimeSpan(1, 34, 21));
            activity.SetupGet(a => a.TotalDistanceMetersEntered).Returns(15.2F);
            activity.SetupGet(a => a.TotalAscendMetersEntered).Returns(460.5F);
            var shoes = new Mock<IEquipmentItem>();
            shoes.SetupGet(s => s.ReferenceId).Returns("783cc7b1-4a73-4b3f-b90f-7c96282406a3");
            var equipment = new List<IEquipmentItem>();
            equipment.Add(shoes.Object);
            activity.SetupGet(a => a.EquipmentUsed).Returns(equipment);
            activity.SetupGet(a => a.AverageHeartRatePerMinuteEntered).Returns(170F);
            activity.SetupGet(a => a.MaximumHeartRatePerMinuteEntered).Returns(186F);

            _config.Profile.AdvancedFeaturesEnabled = true;
            var action = new ExportTrainingAction(activity.Object);
            var note = new ApTraining();
            var edata = new ExportConfig()
            {
                ActivityData = data,
                Logbook = logbook.Object,
                Metadata = _metadata,
                Config = _config
            };

            var error = action.Populate(note, activity.Object, edata);

            Assert.Equal(null, error);
            AssertDate(note, date);
            AssertAthlete(note);
            Assert.Equal("This is a note", note.Description);

            // Assert training
            Assert.Equal("013421", note.TotalTime);
            Assert.Equal("-1", note.IntensityId);
            Assert.Equal("10", note.Intensity0);
            Assert.Equal("11", note.Intensity1);
            Assert.Equal("12", note.Intensity2);
            Assert.Equal("13", note.Intensity3);
            Assert.Equal("14", note.Intensity4);
            Assert.Equal("15", note.Intensity5);
            Assert.Equal("5785", note.ActivityId);
            Assert.Equal("long", note.ActivitySubType);
            Assert.Equal("1", note.WorkoutId);
            Assert.Equal("2602", note.ShoesId);
            Assert.Equal("170", note.AverageHeartRate);
            Assert.Equal("186", note.MaxHeartRate);
            Assert.Equal("16", note.SpikedControls);
            Assert.Equal("20", note.TotalControls);
            Assert.Equal("4", note.TechnicalIntensityId);
        }

        [Fact]
        public void ExportTrainingWithOverridenData() {
            var date = new DateTime(2009, 7, 11);

            var data = new ApActivityData()
            {
                SpikedControls = "16",
                TotalControls = "20",
                TechnicalIntensityId = "4",
                // Override workoutId and subtype
                WorkoutId = "2",
                ActivitySubtype = "longish"
            };

            Mock<IAthlete> athlete;
            var logbook = SetUpLogbook(DateTime.MinValue, out athlete);

            var activity = new Mock<IActivity>();
            activity.SetupGet(a => a.StartTime).Returns(date);
            activity.SetupGet(a => a.Notes).Returns("This is a note");

            // Setup training
            var category = new Mock<IActivityCategory>();
            category.SetupGet(c => c.ReferenceId).Returns("5e13c6e5-59d5-4e99-9fbe-16c65e1111e1");
            activity.SetupGet(a => a.Category).Returns(category.Object);
            activity.SetupGet(a => a.TotalTimeEntered).Returns(new TimeSpan(1, 34, 21));
            activity.SetupGet(a => a.Intensity).Returns(8);
            activity.SetupGet(a => a.TotalDistanceMetersEntered).Returns(15210.2F);
            activity.SetupGet(a => a.TotalAscendMetersEntered).Returns(460.5F);
            var shoes = new Mock<IEquipmentItem>();
            shoes.SetupGet(s => s.ReferenceId).Returns("783cc7b1-4a73-4b3f-b90f-7c96282406a3");
            var equipment = new List<IEquipmentItem>();
            equipment.Add(shoes.Object);
            activity.SetupGet(a => a.EquipmentUsed).Returns(equipment);
            activity.SetupGet(a => a.AverageHeartRatePerMinuteEntered).Returns(170F);
            activity.SetupGet(a => a.MaximumHeartRatePerMinuteEntered).Returns(186F);

            var action = new ExportTrainingAction(activity.Object);
            var note = new ApTraining();
            var edata = new ExportConfig()
            {
                ActivityData = data,
                Logbook = logbook.Object,
                Metadata = _metadata,
                Config = _config
            };

            var error = action.Populate(note, activity.Object, edata);

            Assert.Equal(null, error);
            AssertDate(note, date);
            AssertAthlete(note);
            Assert.Equal("This is a note", note.Description);

            // Assert training
            Assert.Equal("013421", note.TotalTime);
            Assert.Equal("15.21", note.Distance);
            Assert.Equal("kilometers", note.DistanceUnitId);
            Assert.Equal("460.5", note.Climb);
            Assert.Equal("m", note.ClimbUnitId);
            Assert.Equal("4", note.IntensityId);
            Assert.Equal("5785", note.ActivityId);
            Assert.Equal("longish", note.ActivitySubType);
            Assert.Equal("2", note.WorkoutId);
            Assert.Equal("2602", note.ShoesId);
            Assert.Equal("170", note.AverageHeartRate);
            Assert.Equal("186", note.MaxHeartRate);
            Assert.Equal("16", note.SpikedControls);
            Assert.Equal("20", note.TotalControls);
            Assert.Equal("4", note.TechnicalIntensityId);
        }

        [Fact]
        public void ExportTrainingWithCategoryNotFound() {
            var date = new DateTime(2009, 7, 11);
            var data = new ApActivityData();

            Mock<IAthlete> athlete;
            var logbook = SetUpLogbook(DateTime.MinValue, out athlete);

            var activity = new Mock<IActivity>();
            activity.SetupGet(a => a.StartTime).Returns(date);

            // Setup training
            var category = new Mock<IActivityCategory>();
            category.SetupGet(c => c.ReferenceId).Returns("5e13c6e5-59d5-4e99-9fbe-16c65e1111e1");
            activity.SetupGet(a => a.Category).Returns(category.Object);

            var action = new ExportTrainingAction(activity.Object);
            var note = new ApTraining();
            // Remove category to emulate the error
            _config.Mapping.Activities.RemoveAt(11);
            var edata = new ExportConfig()
            {
                ActivityData = data,
                Logbook = logbook.Object,
                Metadata = _metadata,
                Config = _config
            };

            var error = action.Populate(note, activity.Object, edata);

            Assert.Equal(ExportError.CategoryNotFound, error);
        }

        [Fact]
        public void ExportTrainingWithCategoryNotMapped() {
            var date = new DateTime(2009, 7, 11);
            var data = new ApActivityData();

            Mock<IAthlete> athlete;
            var logbook = SetUpLogbook(DateTime.MinValue, out athlete);

            var activity = new Mock<IActivity>();
            activity.SetupGet(a => a.StartTime).Returns(date);

            // Setup training
            var category = new Mock<IActivityCategory>();
            category.SetupGet(c => c.ReferenceId).Returns("11c33498-06e8-4c9b-aae2-a7c74afb0b27");
            activity.SetupGet(a => a.Category).Returns(category.Object);
            activity.SetupGet(a => a.TotalTimeEntered).Returns(new TimeSpan(1, 34, 21));

            var action = new ExportTrainingAction(activity.Object);
            var note = new ApTraining();
            var edata = new ExportConfig()
            {
                ActivityData = data,
                Logbook = logbook.Object,
                Metadata = _metadata,
                Config = _config
            };

            var error = action.Populate(note, activity.Object, edata);

            Assert.Equal(ExportError.CategoryNotMapped, error);
        }

        [Fact]
        public void ExportTrainingWithTimeNotSpecified() {
            var date = new DateTime(2009, 7, 11);
            var data = new ApActivityData();

            Mock<IAthlete> athlete;
            var logbook = SetUpLogbook(DateTime.MinValue, out athlete);

            var activity = new Mock<IActivity>();
            activity.SetupGet(a => a.StartTime).Returns(date);

            // Setup training
            var category = new Mock<IActivityCategory>();
            category.SetupGet(c => c.ReferenceId).Returns("5e13c6e5-59d5-4e99-9fbe-16c65e1111e1");
            activity.SetupGet(a => a.Category).Returns(category.Object);

            var action = new ExportTrainingAction(activity.Object);
            var note = new ApTraining();
            var edata = new ExportConfig()
            {
                ActivityData = data,
                Logbook = logbook.Object,
                Metadata = _metadata,
                Config = _config
            };

            var error = action.Populate(note, activity.Object, edata);

            Assert.Equal(ExportError.TimeNotSpecified, error);
        }

        [Fact]
        public void ExportTrainingWithIntensityNotFound() {
            var date = new DateTime(2009, 7, 11);
            var data = new ApActivityData();

            Mock<IAthlete> athlete;
            var logbook = SetUpLogbook(DateTime.MinValue, out athlete);

            var activity = new Mock<IActivity>();
            activity.SetupGet(a => a.StartTime).Returns(date);

            // Setup training
            var category = new Mock<IActivityCategory>();
            category.SetupGet(c => c.ReferenceId).Returns("5e13c6e5-59d5-4e99-9fbe-16c65e1111e1");
            activity.SetupGet(a => a.Category).Returns(category.Object);
            activity.SetupGet(a => a.TotalTimeEntered).Returns(new TimeSpan(1, 34, 21));
            activity.SetupGet(a => a.Intensity).Returns(2);

            var action = new ExportTrainingAction(activity.Object);
            var note = new ApTraining();
            var edata = new ExportConfig()
            {
                ActivityData = data,
                Logbook = logbook.Object,
                Metadata = _metadata,
                Config = _config
            };

            // Remove intensity to emulate the error
            _config.Mapping.Intensities.RemoveAt(2);
            var error = action.Populate(note, activity.Object, edata);

            Assert.Equal(ExportError.IntensityNotFound, error);
        }

        [Fact]
        public void ExportTrainingWithIntensityNotMappedForNotAdvancedAccount() {
            var date = new DateTime(2009, 7, 11);
            var data = new ApActivityData();

            Mock<IAthlete> athlete;
            var logbook = SetUpLogbook(DateTime.MinValue, out athlete);

            var activity = new Mock<IActivity>();
            activity.SetupGet(a => a.StartTime).Returns(date);

            // Setup training
            var category = new Mock<IActivityCategory>();
            category.SetupGet(c => c.ReferenceId).Returns("5e13c6e5-59d5-4e99-9fbe-16c65e1111e1");
            activity.SetupGet(a => a.Category).Returns(category.Object);
            activity.SetupGet(a => a.TotalTimeEntered).Returns(new TimeSpan(1, 34, 21));
            activity.SetupGet(a => a.Intensity).Returns(5);

            _config.Profile.AdvancedFeaturesEnabled = false;
            var action = new ExportTrainingAction(activity.Object);
            var note = new ApTraining();
            var edata = new ExportConfig()
            {
                ActivityData = data,
                Logbook = logbook.Object,
                Metadata = _metadata,
                Config = _config
            };

            var error = action.Populate(note, activity.Object, edata);

            Assert.Equal(ExportError.IntensityNotMapped, error);
        }

        [Fact]
        public void ExportTrainingWithIntensityNotMappedForAdvancedAccount() {
            var date = new DateTime(2009, 7, 11);
            var data = new ApActivityData();

            Mock<IAthlete> athlete;
            var logbook = SetUpLogbook(DateTime.MinValue, out athlete);

            var activity = new Mock<IActivity>();
            activity.SetupGet(a => a.StartTime).Returns(date);

            // Setup training
            var category = new Mock<IActivityCategory>();
            category.SetupGet(c => c.ReferenceId).Returns("5e13c6e5-59d5-4e99-9fbe-16c65e1111e1");
            activity.SetupGet(a => a.Category).Returns(category.Object);
            activity.SetupGet(a => a.TotalTimeEntered).Returns(new TimeSpan(1, 34, 21));
            activity.SetupGet(a => a.Intensity).Returns(5);

            _config.Profile.AdvancedFeaturesEnabled = true;
            var action = new ExportTrainingAction(activity.Object);
            var note = new ApTraining();
            var edata = new ExportConfig()
            {
                ActivityData = data,
                Logbook = logbook.Object,
                Metadata = _metadata,
                Config = _config
            };

            var error = action.Populate(note, activity.Object, edata);

            Assert.Equal(ExportError.IntensityNotMapped, error);
        }

        [Fact]
        public void ExportTrainingWithDistanceNotSpecified() {
            var date = new DateTime(2009, 7, 11);
            var data = new ApActivityData();

            Mock<IAthlete> athlete;
            var logbook = SetUpLogbook(DateTime.MinValue, out athlete);

            var activity = new Mock<IActivity>();
            activity.SetupGet(a => a.StartTime).Returns(date);

            // Setup training
            var category = new Mock<IActivityCategory>();
            category.SetupGet(c => c.ReferenceId).Returns("5e13c6e5-59d5-4e99-9fbe-16c65e1111e1");
            activity.SetupGet(a => a.Category).Returns(category.Object);
            activity.SetupGet(a => a.TotalTimeEntered).Returns(new TimeSpan(1, 34, 21));
            activity.SetupGet(a => a.Intensity).Returns(8);

            var action = new ExportTrainingAction(activity.Object);
            var note = new ApTraining();
            var edata = new ExportConfig()
            {
                ActivityData = data,
                Logbook = logbook.Object,
                Metadata = _metadata,
                Config = _config
            };

            var error = action.Populate(note, activity.Object, edata);

            Assert.Equal(ExportError.DistanceNotSpecified, error);
        }

        [Fact]
        public void ExportTrainingWithEquipmentNotFound() {
            var date = new DateTime(2009, 7, 11);
            var data = new ApActivityData();

            Mock<IAthlete> athlete;
            var logbook = SetUpLogbook(DateTime.MinValue, out athlete);

            var activity = new Mock<IActivity>();
            activity.SetupGet(a => a.StartTime).Returns(date);

            // Setup training
            var category = new Mock<IActivityCategory>();
            category.SetupGet(c => c.ReferenceId).Returns("5e13c6e5-59d5-4e99-9fbe-16c65e1111e1");
            activity.SetupGet(a => a.Category).Returns(category.Object);
            activity.SetupGet(a => a.TotalTimeEntered).Returns(new TimeSpan(1, 34, 21));
            activity.SetupGet(a => a.Intensity).Returns(8);
            activity.SetupGet(a => a.TotalDistanceMetersEntered).Returns(15.2F);
            var shoes = new Mock<IEquipmentItem>();
            shoes.SetupGet(s => s.ReferenceId).Returns("783cc7b1-4a73-4b3f-b90f-7c96282406a3");
            var equipment = new List<IEquipmentItem>();
            equipment.Add(shoes.Object);
            activity.SetupGet(a => a.EquipmentUsed).Returns(equipment);

            // Remove equipment to emulate the error
            _config.Mapping.Shoes.RemoveAt(5);
            var action = new ExportTrainingAction(activity.Object);
            var note = new ApTraining();
            var edata = new ExportConfig()
            {
                ActivityData = data,
                Logbook = logbook.Object,
                Metadata = _metadata,
                Config = _config
            };

            var error = action.Populate(note, activity.Object, edata);

            Assert.Equal(ExportError.EquipmentNotFound, error);
        }

        [Fact]
        public void ExportTrainingWithEquipmentNotMappedWithWarning() {
            var date = new DateTime(2009, 7, 11);
            var data = new ApActivityData();

            Mock<IAthlete> athlete;
            var logbook = SetUpLogbook(DateTime.MinValue, out athlete);

            var activity = new Mock<IActivity>();
            activity.SetupGet(a => a.StartTime).Returns(date);

            // Setup training
            var category = new Mock<IActivityCategory>();
            category.SetupGet(c => c.ReferenceId).Returns("5e13c6e5-59d5-4e99-9fbe-16c65e1111e1");
            activity.SetupGet(a => a.Category).Returns(category.Object);
            activity.SetupGet(a => a.TotalTimeEntered).Returns(new TimeSpan(1, 34, 21));
            activity.SetupGet(a => a.Intensity).Returns(8);
            activity.SetupGet(a => a.TotalDistanceMetersEntered).Returns(15.2F);
            var shoes = new Mock<IEquipmentItem>();
            shoes.SetupGet(s => s.ReferenceId).Returns("792d3504-ea35-43e0-942b-99d97195a236");
            var equipment = new List<IEquipmentItem>();
            equipment.Add(shoes.Object);
            activity.SetupGet(a => a.EquipmentUsed).Returns(equipment);

            // Turn on the warning 
            _config.WarnOnNotMappedEquipment = true;
            var action = new ExportTrainingAction(activity.Object);
            var note = new ApTraining();
            var edata = new ExportConfig()
            {
                ActivityData = data,
                Logbook = logbook.Object,
                Metadata = _metadata,
                Config = _config
            };

            var error = action.Populate(note, activity.Object, edata);

            Assert.Equal(null, error);
            Assert.NotEqual(0, (int)(edata.Warnings & ExportWarning.EquipmentNotMapped));
        }

        [Fact]
        public void ExportTrainingWithEquipmentNotMappedWithoutWarning() {
            var date = new DateTime(2009, 7, 11);
            var data = new ApActivityData();

            Mock<IAthlete> athlete;
            var logbook = SetUpLogbook(DateTime.MinValue, out athlete);

            var activity = new Mock<IActivity>();
            activity.SetupGet(a => a.StartTime).Returns(date);

            // Setup training
            var category = new Mock<IActivityCategory>();
            category.SetupGet(c => c.ReferenceId).Returns("5e13c6e5-59d5-4e99-9fbe-16c65e1111e1");
            activity.SetupGet(a => a.Category).Returns(category.Object);
            activity.SetupGet(a => a.TotalTimeEntered).Returns(new TimeSpan(1, 34, 21));
            activity.SetupGet(a => a.Intensity).Returns(8);
            activity.SetupGet(a => a.TotalDistanceMetersEntered).Returns(15.2F);
            var shoes = new Mock<IEquipmentItem>();
            shoes.SetupGet(s => s.ReferenceId).Returns("792d3504-ea35-43e0-942b-99d97195a236");
            var equipment = new List<IEquipmentItem>();
            equipment.Add(shoes.Object);
            activity.SetupGet(a => a.EquipmentUsed).Returns(equipment);

            // Turn off the warning 
            _config.WarnOnNotMappedEquipment = false;
            var action = new ExportTrainingAction(activity.Object);
            var note = new ApTraining();
            var edata = new ExportConfig()
            {
                ActivityData = data,
                Logbook = logbook.Object,
                Metadata = _metadata,
                Config = _config
            };

            var error = action.Populate(note, activity.Object, edata);

            // Still it must be a warning even when the warning is off
            Assert.Equal(null, error);
            Assert.NotEqual(0, (int)(edata.Warnings & ExportWarning.EquipmentNotMapped));
        }

        [Fact]
        public void ExportTrainingWithOptionalNullProperties() {
            var date = new DateTime(2009, 7, 11);
            var data = new ApActivityData();

            Mock<IAthlete> athlete;
            var logbook = SetUpLogbook(DateTime.MinValue, out athlete);

            var activity = new Mock<IActivity>();
            activity.SetupGet(a => a.StartTime).Returns(date);

            // Setup training
            var category = new Mock<IActivityCategory>();
            category.SetupGet(c => c.ReferenceId).Returns("5e13c6e5-59d5-4e99-9fbe-16c65e1111e1");
            activity.SetupGet(a => a.Category).Returns(category.Object);
            activity.SetupGet(a => a.TotalTimeEntered).Returns(new TimeSpan(1, 34, 21));
            activity.SetupGet(a => a.Intensity).Returns(8);
            activity.SetupGet(a => a.TotalDistanceMetersEntered).Returns(15.2F);
            var shoes = new Mock<IEquipmentItem>();
            shoes.SetupGet(s => s.ReferenceId).Returns("783cc7b1-4a73-4b3f-b90f-7c96282406a3");
            var equipment = new List<IEquipmentItem>();
            equipment.Add(shoes.Object);
            activity.SetupGet(a => a.EquipmentUsed).Returns(equipment);

            var action = new ExportTrainingAction(activity.Object);
            var note = new ApTraining();
            var edata = new ExportConfig()
            {
                ActivityData = data,
                Logbook = logbook.Object,
                Metadata = _metadata,
                Config = _config
            };

            var error = action.Populate(note, activity.Object, edata);

            Assert.Equal(null, error);
            AssertDate(note, date);
            AssertAthlete(note);
            Assert.Equal(null, note.Description);

            // Assert training
            Assert.Equal("013421", note.TotalTime);
            Assert.Equal("4", note.IntensityId);
            Assert.Equal("5785", note.ActivityId);
            Assert.Equal("long", note.ActivitySubType);
            Assert.Equal("1", note.WorkoutId);
            Assert.Equal("2602", note.ShoesId);
            Assert.Equal(null, note.AverageHeartRate);
            Assert.Equal(null, note.MaxHeartRate);
            Assert.Equal(null, note.SpikedControls);
            Assert.Equal(null, note.TotalControls);
            Assert.Equal(null, note.TechnicalIntensityId);
        }

        [Fact]
        public void ExportTrainingWithOptionalNanProperties() {
            var date = new DateTime(2009, 7, 11);
            var data = new ApActivityData();

            Mock<IAthlete> athlete;
            var logbook = SetUpLogbook(DateTime.MinValue, out athlete);

            var activity = new Mock<IActivity>();
            activity.SetupGet(a => a.StartTime).Returns(date);

            // Setup training
            var category = new Mock<IActivityCategory>();
            category.SetupGet(c => c.ReferenceId).Returns("5e13c6e5-59d5-4e99-9fbe-16c65e1111e1");
            activity.SetupGet(a => a.Category).Returns(category.Object);
            activity.SetupGet(a => a.TotalTimeEntered).Returns(new TimeSpan(1, 34, 21));
            activity.SetupGet(a => a.Intensity).Returns(8);
            activity.SetupGet(a => a.TotalDistanceMetersEntered).Returns(15210.2F);
            var shoes = new Mock<IEquipmentItem>();
            shoes.SetupGet(s => s.ReferenceId).Returns("783cc7b1-4a73-4b3f-b90f-7c96282406a3");
            var equipment = new List<IEquipmentItem>();
            equipment.Add(shoes.Object);
            activity.SetupGet(a => a.EquipmentUsed).Returns(equipment);
            activity.SetupGet(a => a.AverageHeartRatePerMinuteEntered).Returns(float.NaN);
            activity.SetupGet(a => a.MaximumHeartRatePerMinuteEntered).Returns(float.NaN);
            activity.SetupGet(a => a.TotalAscendMetersEntered).Returns(float.NaN);

            var action = new ExportTrainingAction(activity.Object);
            var note = new ApTraining();
            var edata = new ExportConfig()
            {
                ActivityData = data,
                Logbook = logbook.Object,
                Metadata = _metadata,
                Config = _config
            };

            var error = action.Populate(note, activity.Object, edata);

            Assert.Equal(null, error);
            AssertDate(note, date);
            AssertAthlete(note);
            Assert.Equal(null, note.Description);

            // Assert training
            Assert.Equal("013421", note.TotalTime);
            Assert.Equal("15.21", note.Distance);
            Assert.Equal("kilometers", note.DistanceUnitId);
            Assert.Equal(null, note.Climb);
            Assert.Equal(null, note.ClimbUnitId);
            Assert.Equal("4", note.IntensityId);
            Assert.Equal("5785", note.ActivityId);
            Assert.Equal("long", note.ActivitySubType);
            Assert.Equal("1", note.WorkoutId);
            Assert.Equal("2602", note.ShoesId);
            Assert.Equal(null, note.AverageHeartRate);
            Assert.Equal(null, note.MaxHeartRate);
            Assert.Equal(null, note.SpikedControls);
            Assert.Equal(null, note.TotalControls);
            Assert.Equal(null, note.TechnicalIntensityId);
        }

        [Fact]
        public void ExportTrainingWithFormattedDescription() {
            var date = new DateTime(2009, 7, 11);

            var data = new ApActivityData()
            {
                CourseName = "Blue",
                CourseLength = "10.2",
                CourseClimb = "350"
            };
            // Description format
            _config.NotesFormat = "[{Name}] [in {Location}.] [Burned {Calories}.]\r\n[{CourseSpec}]\r\n[{Notes}]";

            Mock<IAthlete> athlete;
            var logbook = SetUpLogbook(DateTime.MinValue, out athlete);

            var activity = new Mock<IActivity>();
            activity.SetupGet(a => a.StartTime).Returns(date);
            activity.SetupGet(a => a.Notes).Returns("This is a note");
            activity.SetupGet(a => a.Name).Returns("Evening run");
            activity.SetupGet(a => a.Location).Returns("Rancho San Antonio");
            activity.SetupGet(a => a.TotalCalories).Returns(350.4F);

            // Setup training
            var category = new Mock<IActivityCategory>();
            category.SetupGet(c => c.ReferenceId).Returns("5e13c6e5-59d5-4e99-9fbe-16c65e1111e1");
            activity.SetupGet(a => a.Category).Returns(category.Object);
            activity.SetupGet(a => a.TotalTimeEntered).Returns(new TimeSpan(1, 34, 21));
            activity.SetupGet(a => a.Intensity).Returns(8);
            activity.SetupGet(a => a.TotalDistanceMetersEntered).Returns(15210.2F);
            var shoes = new Mock<IEquipmentItem>();
            shoes.SetupGet(s => s.ReferenceId).Returns("783cc7b1-4a73-4b3f-b90f-7c96282406a3");
            var equipment = new List<IEquipmentItem>();
            equipment.Add(shoes.Object);
            activity.SetupGet(a => a.EquipmentUsed).Returns(equipment);

            var action = new ExportTrainingAction(activity.Object);
            var note = new ApTraining();
            var edata = new ExportConfig()
            {
                ActivityData = data,
                Logbook = logbook.Object,
                Metadata = _metadata,
                Config = _config
            };

            var error = action.Populate(note, activity.Object, edata);

            Assert.Equal(null, error);
            Assert.Equal("Evening run in Rancho San Antonio. Burned 350.4.\r\nCourse: Blue 10.2 km 350 m\r\nThis is a note", note.Description);

        }

        private void AssertDate(ApTraining note, DateTime date) {
            Assert.Equal(date, note.Date);
            Assert.Equal("2009", note.Year);
            Assert.Equal("07", note.MonthId);
            Assert.Equal("11", note.DayId);
        }

        private void AssertAthlete(ApTraining note) {
            Assert.Equal(null, note.IsSick);
            Assert.Equal(null, note.IsInjured);
            Assert.Equal(null, note.IsRestDay);
            Assert.Equal(null, note.RestingHeartRate);
            Assert.Equal(null, note.Weight);
            Assert.Equal(null, note.WeightUnitId);
            Assert.Equal(null, note.SleepHours);
        }

    }
}
