using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using GK.SportTracks.AttackPoint;
using Moq;

namespace AttackPointPluginTests
{
    public class Test_MapperCategory : TestBase_Config
    {

        [Fact]
        public void MapCategoryWithNullActivities() {
            _profile.Activities = null;
            Assert.Null(Mapper.MapCategory(_profile, GetStCategory("", ""), false));
        }

        [Fact]
        public void MapCategoryWithZeroActivities() {
            _profile.Activities.Clear();
            Assert.Null(Mapper.MapCategory(_profile, GetStCategory("", ""), false));
        }

        [Fact]
        public void MapCategory() {
            var activity = Mapper.MapCategory(_profile, GetStCategory("", "5999"), false);
            Assert.NotNull(activity);
            Assert.Equal("Hiking", activity.Title);
        }

        [Fact]
        public void MapCategoryWithUnmapped() {
            var activity = Mapper.MapCategory(_profile, GetStCategory("", "-1"), false);
            Assert.Null(activity);
        }

        [Fact]
        public void MapCategoryWithGuess1() {
            var activity = Mapper.MapCategory(_profile, GetStCategory("Hiking", "-1"), true);
            Assert.NotNull(activity);
            Assert.Equal("5999", activity.Id);
        }

        [Fact]
        public void MapCategoryWithGuess2() {
            var activity = Mapper.MapCategory(_profile, GetStCategory("Orienteering Relay", "-1"), true);
            Assert.NotNull(activity);
            Assert.Equal("5785", activity.Id);
        }

        [Fact]
        public void MapCategoryWithGuess3() {
            var activity = Mapper.MapCategory(_profile, GetStCategory("Running Terrain", "-1"), true);
            Assert.NotNull(activity);
            Assert.Equal("7446", activity.Id);
        }


        private StCategory GetStCategory(string name, string apId) {
            var category = new Mock<StCategory>();
            category.Setup(c => c.ToString()).Returns(name);
            category.Object.ApId = apId;
            return category.Object;
        }


    }
}
