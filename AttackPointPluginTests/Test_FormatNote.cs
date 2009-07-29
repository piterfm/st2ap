using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using GK.SportTracks.AttackPoint.Export;

namespace AttackPointPluginTests
{
    public class Test_FormatNote
    {
        [Fact]
        public void FormatNote1() {
            var fields = new Dictionary<string, string>();

            fields.Add("Name", "Evening run");
            fields.Add("Location", "Rancho San Antonio");
            fields.Add("Calories", "456");

            var format = "[{TimeOfDay}. ][Happened in {Location}. ][Burned {Calories} calories.][\r\n{CourseSpec}]";

            Assert.Equal("Happened in Rancho San Antonio. Burned 456 calories.", ExportNoteAction.FormatNotes(format, fields, null));

        }

        [Fact]
        public void FormatNote2() {
            var fields = new Dictionary<string, string>();

            fields.Add("Name", "Club chams");
            fields.Add("Location", "Tilden Park, Berkeley, CA");
            fields.Add("CourseSpec", "Blue 7.2 km, 350 m");

            var format = "[{TimeOfDay}. ][Happened in {Location}. ][Burned {Calories} calories.]\r\n[{CourseSpec}] ";

            Assert.Equal("Happened in Tilden Park, Berkeley, CA. \r\nBlue 7.2 km, 350 m ", ExportNoteAction.FormatNotes(format, fields, null));

        }

    }
}
