using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using GK.SportTracks.AttackPoint;
using Moq;

namespace AttackPointPluginTests
{
    public class Test_MapperIntensities : TestBase_Config
    {

        [Fact]
        public void MapIntensityWithZeroIntensities() {
            _profile.Intensities.Clear();
            Assert.Null(Mapper.MapIntensity(_profile, GetStIntensity("", ""), false));
        }

        [Fact]
        public void MapIntensityWithUnmapped() {
            Assert.Null(Mapper.MapIntensity(_profile, GetStIntensity("", "-1"), false));
        }

        [Fact]
        public void MapIntensityWithGuess1() {
            var i = Mapper.MapIntensity(_profile, GetStIntensity("", "5"), true);
            Assert.NotNull(i);
            Assert.Equal("3", i.Id);
        }

        [Fact]
        public void MapIntensity() {
            var i = Mapper.MapIntensity(_profile, GetStIntensity("4", "8"), false);
            Assert.NotNull(i);
            Assert.Equal("4", i.Id);
        }


        private StIntensity GetStIntensity(string apId, string stId) {
            return new StIntensity() { ApId = apId, StId = stId };
        }


    }
}
