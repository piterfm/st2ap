using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using GK.SportTracks.AttackPoint.Export;
using Moq;
using ZoneFiveSoftware.Common.Data.Fitness;
using System.IO;
using System.Xml.Serialization;
using GK.AttackPoint;
using GK.SportTracks.AttackPoint;

namespace AttackPointPluginTests
{
    public abstract class BaseTest_ExportAction : BaseTest
    {
        protected Mock<ILogbook> SetUpLogbook(DateTime date, out Mock<IAthlete> athlete) {
            athlete = SetUpAthlete(date);
            var logbook = new Mock<ILogbook>();
            logbook.Setup(l => l.Athlete).Returns(athlete.Object);
            return logbook;
        }

        protected Mock<ILogbook> SetUpLogbookWithNanAthlete(DateTime date, out Mock<IAthlete> athlete) {
            athlete = SetUpAthleteWithNanValues(date);
            var logbook = new Mock<ILogbook>();
            logbook.Setup(l => l.Athlete).Returns(athlete.Object);
            return logbook;
        }

        protected Mock<IAthlete> SetUpAthlete(DateTime date) {
            var athlete = new Mock<IAthlete>();
            var entry = date == DateTime.MinValue ? null : new Mock<IAthleteInfoEntry>();
            if (entry != null) {
                entry.SetupGet(e => e.Injured).Returns(true);
                entry.SetupGet(e => e.Sick).Returns(true);
                entry.SetupGet(e => e.SleepHours).Returns(8F);
                entry.SetupGet(e => e.RestingHeartRatePerMinute).Returns(55F);
                entry.SetupGet(e => e.WeightKilograms).Returns(64F);
            }

            var entries = new Mock<IAthleteInfoEntries>();
            if (entry != null) {
                entries.Setup(e => e.EntryForDate(date)).Returns(entry.Object);
            }
            athlete.Setup(a => a.InfoEntries).Returns(entries.Object);

            return athlete;
        }

        protected Mock<IAthlete> SetUpAthleteWithNanValues(DateTime date) {
            var athlete = new Mock<IAthlete>();
            var entry = new Mock<IAthleteInfoEntry>();
            entry.SetupGet(e => e.Injured).Returns(true);
            entry.SetupGet(e => e.Sick).Returns(true);
            entry.SetupGet(e => e.SleepHours).Returns(float.NaN);
            entry.SetupGet(e => e.RestingHeartRatePerMinute).Returns(float.NaN);
            entry.SetupGet(e => e.WeightKilograms).Returns(float.NaN);

            var entries = new Mock<IAthleteInfoEntries>();
            entries.Setup(e => e.EntryForDate(date)).Returns(entry.Object);
            athlete.Setup(a => a.InfoEntries).Returns(entries.Object);

            return athlete;
        }
    }
}
