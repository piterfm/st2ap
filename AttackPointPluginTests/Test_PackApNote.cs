using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using GK.AttackPoint;
using System.IO;
using System.Xml.Serialization;

namespace AttackPointPluginTests
{
    public class Test_PackApNote : TestBase
    {
        private ApMetadata _metadata;

        public Test_PackApNote() {
            using (var reader = new StreamReader("ap-metadata.xml")) {
                _metadata = (ApMetadata)new XmlSerializer(typeof(ApMetadata)).Deserialize(reader);
            }
        }

        [Fact]
        public void Pack_NoteWithAllNonNullProperties() {
            var date = new DateTime(2009, 07, 11);
            var note = new ApNote()
            {
                Date = date,
                Weight = "136",
                SleepHours = "8",
                RestingHeartRate = "50",
                WeightUnitId = "kilo",
                IsInjured = "on",
                IsRestDay = "on",
                IsSick = "on",
                Description = "Taking a day off",
                PrivateDescription = "private note"
            };

            var operation = _metadata.Operations[0];
            var parameters = note.Pack(operation);

            Assert.Equal(12, parameters.Count);
            Assert.Equal("07", parameters["session-month"]);
            Assert.Equal("11", parameters["session-day"]);
            Assert.Equal("2009", parameters["session-year"]);
            Assert.Equal("50", parameters["rhr"]);
            Assert.Equal("8", parameters["sleep"]);
            Assert.Equal("136", parameters["weight"]);
            Assert.Equal("on", parameters["injured"]);
            Assert.Equal("on", parameters["sick"]);
            Assert.Equal("on", parameters["restday"]);
            Assert.Equal("Taking a day off", parameters["description"]);
            Assert.Equal("private note", parameters["pdescription"]);
            Assert.Equal("kilo", parameters["wunit"]);
        }

        [Fact]
        public void Pack_EmptyNote() {
            var note = new ApNote();

            var operation = _metadata.Operations[0];
            var parameters = note.Pack(operation);

            Assert.Equal(4, parameters.Count);
            Assert.Equal("", parameters["rhr"]);
            Assert.Equal("", parameters["sleep"]);
            Assert.Equal("", parameters["weight"]);
            Assert.Equal("", parameters["description"]);
        }

        [Fact]
        public void Pack_NoteWithSomeNonNullProperties() {
            var date = new DateTime(2009, 07, 11);
            var note = new ApNote()
            {
                Date = date,
                Weight = "136",
                SleepHours = "8",
                RestingHeartRate = "50",
                WeightUnitId = "kilo",
                Description = "Some text"
            };

            var operation = _metadata.Operations[0];
            var parameters = note.Pack(operation);

            Assert.Equal(8, parameters.Count);
            Assert.Equal("07", parameters["session-month"]);
            Assert.Equal("11", parameters["session-day"]);
            Assert.Equal("2009", parameters["session-year"]);
            Assert.Equal("50", parameters["rhr"]);
            Assert.Equal("8", parameters["sleep"]);
            Assert.Equal("136", parameters["weight"]);
            Assert.Equal("Some text", parameters["description"]);
            Assert.Equal("kilo", parameters["wunit"]);
        }

        [Fact]
        public void Pack_TrainingWithAllNonNullProperties() {
            var date = new DateTime(2009, 07, 11);
            var note = new ApTraining()
            {
                Date = date,
                Weight = "136",
                SleepHours = "8",
                RestingHeartRate = "50",
                WeightUnitId = "kilo",
                IsInjured = "on",
                IsRestDay = "on",
                IsSick = "on",
                Description = "Some text",
                PrivateDescription = "private note",

                ActivityId = "3000",
                ActivitySubType = "repetitions",
                AverageHeartRate = "180",
                Climb = "10",
                ClimbUnitId = "m",
                Distance = "4.3",
                Pace = "6.2",
                DistanceUnitId = "kilometers",
                IntensityId = "5",
                MaxHeartRate = "199",
                ShoesId = "2000",
                SpikedControls = "18",
                TotalControls = "20",
                TechnicalIntensityId = "2",
                Time = new TimeSpan(2, 53, 45),
                WorkoutId = "3",
                IsPlan = "3",

            };

            var operation = _metadata.Operations[1];
            var parameters = note.Pack(operation);

            Assert.Equal(29, parameters.Count);
            Assert.Equal("07", parameters["session-month"]);
            Assert.Equal("11", parameters["session-day"]);
            Assert.Equal("2009", parameters["session-year"]);
            Assert.Equal("50", parameters["rhr"]);
            Assert.Equal("8", parameters["sleep"]);
            Assert.Equal("136", parameters["weight"]);
            Assert.Equal("on", parameters["injured"]);
            Assert.Equal("on", parameters["sick"]);
            Assert.Equal("on", parameters["restday"]);
            Assert.Equal("Some text", parameters["description"]);
            Assert.Equal("private note", parameters["pdescription"]);
            Assert.Equal("kilo", parameters["wunit"]);

            Assert.Equal("3000", parameters["activitytypeid"]);
            Assert.Equal("repetitions", parameters["activitymodifiers"]);
            Assert.Equal("3", parameters["workouttypeid"]);
            Assert.Equal("5", parameters["intensity"]);
            Assert.Equal("025345", parameters["sessionlength"]);
            Assert.Equal("4.3", parameters["distance"]);
            Assert.Equal("6.2", parameters["pace"]);
            Assert.Equal("kilometers", parameters["distanceunits"]);
            Assert.Equal("10", parameters["climb"]);
            Assert.Equal("m", parameters["cunit"]);
            Assert.Equal("2000", parameters["shoes"]);
            Assert.Equal("180", parameters["ahr"]);
            Assert.Equal("199", parameters["mhr"]);
            Assert.Equal("18", parameters["spiked"]);
            Assert.Equal("20", parameters["controls"]);
            Assert.Equal("2", parameters["map"]);
            Assert.Equal("3", parameters["isplan"]);
        }

        [Fact]
        public void Pack_EmptyTraining() {
            var note = new ApTraining();

            var operation = _metadata.Operations[1];
            var parameters = note.Pack(operation);

            Assert.Equal(7, parameters.Count);
            Assert.Equal("", parameters["description"]);
            Assert.Equal("1", parameters["workouttypeid"]);
            Assert.Equal("-1", parameters["intensity"]);
            Assert.Equal("", parameters["climb"]);
            Assert.Equal("null", parameters["shoes"]);
            Assert.Equal("0", parameters["map"]);
            Assert.Equal("0", parameters["isplan"]);
        }

        [Fact]
        public void Pack_TrainingWithMixedIntensity() {
            var date = new DateTime(2009, 07, 11);
            var note = new ApTraining()
            {
                Date = date,
                Weight = "136",
                SleepHours = "8",
                RestingHeartRate = "50",
                WeightUnitId = "kilo",
                IsInjured = "on",
                IsRestDay = "on",
                IsSick = "on",
                Description = "Some text",
                PrivateDescription = "private note",

                ActivityId = "3000",
                ActivitySubType = "repetitions",
                AverageHeartRate = "180",
                Climb = "10",
                ClimbUnitId = "m",
                Distance = "4.3",
                Pace = "6.2",
                DistanceUnitId = "kilometers",
                Intensity0 = "10",
                Intensity1 = "11",
                Intensity2 = "12",
                Intensity3 = "13",
                Intensity4 = "14",
                Intensity5 = "15",
                MaxHeartRate = "199",
                ShoesId = "2000",
                SpikedControls = "18",
                TotalControls = "20",
                TechnicalIntensityId = "2",
                Time = new TimeSpan(2, 53, 45),
                WorkoutId = "3",
                IsPlan = "3",

            };

            var operation = _metadata.Operations[1];
            var parameters = note.Pack(operation);

            Assert.Equal(35, parameters.Count);
            Assert.Equal("07", parameters["session-month"]);
            Assert.Equal("11", parameters["session-day"]);
            Assert.Equal("2009", parameters["session-year"]);
            Assert.Equal("50", parameters["rhr"]);
            Assert.Equal("8", parameters["sleep"]);
            Assert.Equal("136", parameters["weight"]);
            Assert.Equal("on", parameters["injured"]);
            Assert.Equal("on", parameters["sick"]);
            Assert.Equal("on", parameters["restday"]);
            Assert.Equal("Some text", parameters["description"]);
            Assert.Equal("private note", parameters["pdescription"]);
            Assert.Equal("kilo", parameters["wunit"]);

            Assert.Equal("3000", parameters["activitytypeid"]);
            Assert.Equal("repetitions", parameters["activitymodifiers"]);
            Assert.Equal("3", parameters["workouttypeid"]);
            Assert.Equal("-1", parameters["intensity"]);
            Assert.Equal("10", parameters["intensity0"]);
            Assert.Equal("11", parameters["intensity1"]);
            Assert.Equal("12", parameters["intensity2"]);
            Assert.Equal("13", parameters["intensity3"]);
            Assert.Equal("14", parameters["intensity4"]);
            Assert.Equal("15", parameters["intensity5"]);
            Assert.Equal("025345", parameters["sessionlength"]);
            Assert.Equal("4.3", parameters["distance"]);
            Assert.Equal("6.2", parameters["pace"]);
            Assert.Equal("kilometers", parameters["distanceunits"]);
            Assert.Equal("10", parameters["climb"]);
            Assert.Equal("m", parameters["cunit"]);
            Assert.Equal("2000", parameters["shoes"]);
            Assert.Equal("180", parameters["ahr"]);
            Assert.Equal("199", parameters["mhr"]);
            Assert.Equal("18", parameters["spiked"]);
            Assert.Equal("20", parameters["controls"]);
            Assert.Equal("2", parameters["map"]);
            Assert.Equal("3", parameters["isplan"]);
        }

        public void Pack_TrainingWithSomeMixedIntensity() {
            var date = new DateTime(2009, 07, 11);
            var note = new ApTraining()
            {
                Date = date,
                Weight = "136",
                SleepHours = "8",
                RestingHeartRate = "50",
                WeightUnitId = "kilo",
                IsInjured = "on",
                IsRestDay = "on",
                IsSick = "on",
                Description = "Some text",
                PrivateDescription = "private note",

                ActivityId = "3000",
                ActivitySubType = "repetitions",
                AverageHeartRate = "180",
                Climb = "10",
                ClimbUnitId = "m",
                Distance = "4.3",
                Pace = "6.2",
                DistanceUnitId = "kilometers",
                Intensity1 = "11",
                Intensity2 = "12",
                Intensity5 = "15",
                MaxHeartRate = "199",
                ShoesId = "2000",
                SpikedControls = "18",
                TotalControls = "20",
                TechnicalIntensityId = "2",
                Time = new TimeSpan(2, 53, 45),
                WorkoutId = "3",
                IsPlan = "3",

            };

            var operation = _metadata.Operations[1];
            var parameters = note.Pack(operation);

            Assert.Equal(31, parameters.Count);
            Assert.Equal("07", parameters["session-month"]);
            Assert.Equal("11", parameters["session-day"]);
            Assert.Equal("2009", parameters["session-year"]);
            Assert.Equal("50", parameters["rhr"]);
            Assert.Equal("8", parameters["sleep"]);
            Assert.Equal("136", parameters["weight"]);
            Assert.Equal("on", parameters["injured"]);
            Assert.Equal("on", parameters["sick"]);
            Assert.Equal("on", parameters["restday"]);
            Assert.Equal("Some text", parameters["description"]);
            Assert.Equal("private note", parameters["pdescription"]);
            Assert.Equal("kilo", parameters["wunit"]);

            Assert.Equal("3000", parameters["activitytypeid"]);
            Assert.Equal("repetitions", parameters["activitymodifiers"]);
            Assert.Equal("3", parameters["workouttypeid"]);
            Assert.Equal("-1", parameters["intensity"]);
            Assert.Equal("11", parameters["intensity1"]);
            Assert.Equal("12", parameters["intensity2"]);
            Assert.Equal("15", parameters["intensity5"]);
            Assert.Equal("025345", parameters["sessionlength"]);
            Assert.Equal("4.3", parameters["distance"]);
            Assert.Equal("6.2", parameters["pace"]);
            Assert.Equal("kilometers", parameters["distanceunits"]);
            Assert.Equal("10", parameters["climb"]);
            Assert.Equal("m", parameters["cunit"]);
            Assert.Equal("2000", parameters["shoes"]);
            Assert.Equal("180", parameters["ahr"]);
            Assert.Equal("199", parameters["mhr"]);
            Assert.Equal("18", parameters["spiked"]);
            Assert.Equal("20", parameters["controls"]);
            Assert.Equal("2", parameters["map"]);
            Assert.Equal("3", parameters["isplan"]);
        }

    }
}
