using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using GK.SportTracks.AttackPoint.Export;

namespace AttackPointPluginTests
{
    public class Test_PartOfDay
    {

        [Fact]
        public void Test_Morning() {
            Assert.Equal("Morning", ExportNoteAction.GetPartOfDay(GetTime(7, 32, 10)));
            Assert.Equal("Morning", ExportNoteAction.GetPartOfDay(GetTime(4, 0, 0)));
            Assert.Equal("Morning", ExportNoteAction.GetPartOfDay(GetTime(10, 59, 59)));
        }

        [Fact]
        public void Test_Midday() {
            Assert.Equal("Midday", ExportNoteAction.GetPartOfDay(GetTime(12, 32, 10)));
            Assert.Equal("Midday", ExportNoteAction.GetPartOfDay(GetTime(11, 0, 0)));
            Assert.Equal("Midday", ExportNoteAction.GetPartOfDay(GetTime(13, 59, 59)));
        }

        [Fact]
        public void Test_Afternoon() {
            Assert.Equal("Afternoon", ExportNoteAction.GetPartOfDay(GetTime(16, 32, 10)));
            Assert.Equal("Afternoon", ExportNoteAction.GetPartOfDay(GetTime(14, 0, 0)));
            Assert.Equal("Afternoon", ExportNoteAction.GetPartOfDay(GetTime(17, 29, 59)));
        }

        [Fact]
        public void Test_Evening() {
            Assert.Equal("Evening", ExportNoteAction.GetPartOfDay(GetTime(19, 32, 10)));
            Assert.Equal("Evening", ExportNoteAction.GetPartOfDay(GetTime(17, 30, 0)));
            Assert.Equal("Evening", ExportNoteAction.GetPartOfDay(GetTime(20, 59, 59)));
        }

        [Fact]
        public void Test_Night() {
            Assert.Equal("Night", ExportNoteAction.GetPartOfDay(GetTime(0, 0, 0)));
            Assert.Equal("Night", ExportNoteAction.GetPartOfDay(GetTime(1, 32, 10)));
            Assert.Equal("Night", ExportNoteAction.GetPartOfDay(GetTime(21, 0, 0)));
            Assert.Equal("Night", ExportNoteAction.GetPartOfDay(GetTime(3, 59, 59)));
        }

        private DateTime GetTime(int hours, int minutes, int seconds) {
            return new DateTime(2009, 1, 1, hours, minutes, seconds);
        }
    }
}
