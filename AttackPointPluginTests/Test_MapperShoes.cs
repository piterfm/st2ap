using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using GK.SportTracks.AttackPoint;
using Moq;

namespace AttackPointPluginTests
{
    public class Test_MapperShoes : BaseTest
    {

        [Fact]
        public void MapEquipmentWithNullShoes() {
            _profile.Shoes = null;
            Assert.Null(Mapper.MapEquipment(_profile, GetStEquipment("", ""), false));
        }

        [Fact]
        public void MapEquipmentWithZeroShoes() {
            _profile.Shoes.Clear();
            Assert.Null(Mapper.MapEquipment(_profile, GetStEquipment("", ""), false));
        }

        [Fact]
        public void MapEquipmentWithUnmapped() {
            var shoes = Mapper.MapEquipment(_profile, GetStEquipment("", "-1"), false);
            Assert.Null(shoes);
        }

        [Fact]
        public void MapEquipment() {
            var shoes = Mapper.MapEquipment(_profile, GetStEquipment("", "2602"), false);
            Assert.NotNull(shoes);
            Assert.Equal("Montrail Highlander", shoes.Title);
        }

        [Fact]
        public void MapEquipmentWithGuess1() {
            var shoes = Mapper.MapEquipment(_profile, GetStEquipment("Inov-8 MudClaw 'O' 340", "-1"), true);
            Assert.NotNull(shoes);
            Assert.Equal("2083", shoes.Id);
        }

        [Fact]
        public void MapEquipmentWithGuess2() {
            var shoes = Mapper.MapEquipment(_profile, GetStEquipment("Nike", "-1"), true);
            Assert.NotNull(shoes);
            Assert.Equal("1907", shoes.Id);
        }


        private StEquipment GetStEquipment(string name, string apId) {
            var shoes = new Mock<StEquipment>();
            shoes.Setup(c => c.ToString()).Returns(name);
            shoes.Object.ApId = apId;
            return shoes.Object;
        }


    }
}
