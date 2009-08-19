using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GK.SportTracks.AttackPoint;
using Xunit;

namespace AttackPointPluginTests
{
    public class Test_ApActivityData : TestBase
    {
        [Fact]
        public void TestEmptiness1() {
            var data = new ApActivityData();
            Assert.True(data.IsEmpty());
        }

        [Fact]
        public void TestEmptiness2() {
            var data = new ApActivityData();
            data.WorkoutId = "1";
            Assert.True(data.IsEmpty());
        }

        [Fact]
        public void TestEmptiness3() {
            var data = new ApActivityData();
            data.TechnicalIntensityId = "0";
            Assert.True(data.IsEmpty());
        }

        [Fact]
        public void TestEmptiness4() {
            var data = new ApActivityData();
            data.TechnicalIntensityId = "1";
            Assert.False(data.IsEmpty());
        }

        [Fact]
        public void TestEmptiness5() {
            var data = new ApActivityData();
            data.WorkoutId = "1";
            data.TechnicalIntensityId = "0";
            Assert.True(data.IsEmpty());
        }

        [Fact]
        public void TestEmptiness6() {
            var data = new ApActivityData();
            data.WorkoutId = "2";
            Assert.False(data.IsEmpty());
        }

        [Fact]
        public void TestGetIntensityTime() {
            Assert.Equal(new TimeSpan(12, 34, 54), ApActivityData.GetIntensityTime("123454"));
            Assert.Equal(new TimeSpan(2, 34, 54), ApActivityData.GetIntensityTime("23454"));
            Assert.Equal(new TimeSpan(2, 34, 54), ApActivityData.GetIntensityTime("023454"));
            Assert.Equal(new TimeSpan(0, 34, 54), ApActivityData.GetIntensityTime("3454"));
            Assert.Equal(new TimeSpan(0, 14, 54), ApActivityData.GetIntensityTime("1454"));
            Assert.Equal(new TimeSpan(0, 4, 54), ApActivityData.GetIntensityTime("454"));
            Assert.Equal(new TimeSpan(0, 4, 54), ApActivityData.GetIntensityTime("0454"));
            Assert.Equal(new TimeSpan(0, 0, 54), ApActivityData.GetIntensityTime("54"));
            Assert.Equal(new TimeSpan(0, 0, 4), ApActivityData.GetIntensityTime("4"));
            Assert.Equal(new TimeSpan(0, 0, 4), ApActivityData.GetIntensityTime("04"));
            Assert.Equal(new TimeSpan(0, 1, 4), ApActivityData.GetIntensityTime("64"));
            Assert.Equal(new TimeSpan(1, 5, 3), ApActivityData.GetIntensityTime("6503"));
            Assert.Equal(new TimeSpan(23, 59, 59), ApActivityData.GetIntensityTime("235959"));
        }

        [Fact]
        public void TestGetIntensityTimeError() {
            Assert.Equal(TimeSpan.Zero, ApActivityData.GetIntensityTime(null));
            Assert.Equal(TimeSpan.Zero, ApActivityData.GetIntensityTime(""));
            Assert.Equal(TimeSpan.Zero, ApActivityData.GetIntensityTime("1234567"));
            Assert.Equal(TimeSpan.Zero, ApActivityData.GetIntensityTime("2x67"));
            Assert.Equal(TimeSpan.Zero, ApActivityData.GetIntensityTime("240000"));
            Assert.Equal(TimeSpan.Zero, ApActivityData.GetIntensityTime("325745"));
        }

        [Fact]
        public void TestEmptyMixedIntensity1() {
            var data = new ApActivityData();
            Assert.Equal(false, data.IsMixedIntensitySpecified());
        }

        [Fact]
        public void TestEmptyMixedIntensity2() {
            var data = new ApActivityData();
            for (int i = 0; i <= 5; ++i) {
                data.Intensities[i] = string.Empty;
            }
            Assert.False(data.IsMixedIntensitySpecified());
        }

        [Fact]
        public void TestMixedIntensity1() {
            var data = new ApActivityData();

            data.Intensities[0] = "10023";
            data.Intensities[1] = "0";
            data.Intensities[2] = "45";
            data.Intensities[3] = "1000";
            data.Intensities[4] = "104";
            data.Intensities[5] = "0203";

            Assert.True(data.IsMixedIntensitySpecified());
            Assert.Equal(new TimeSpan(1, 14, 15), data.GetMixedIntensityTime());
        }

        [Fact]
        public void TestMixedIntensity2() {
            var data = new ApActivityData();

            data.Intensities[2] = "2256";
            data.Intensities[3] = "330";
            data.Intensities[4] = "104";

            Assert.True(data.IsMixedIntensitySpecified());
            Assert.Equal(new TimeSpan(0, 27, 30), data.GetMixedIntensityTime());
        }

        [Fact]
        public void TestSingleIntensity1() {
            var data = new ApActivityData();

            data.Intensities[2] = "2256";
            data.Intensities[3] = "330";

            Assert.False(data.IsSingleIntensitySpecified(new TimeSpan(0, 3, 30), 3));
        }

        [Fact]
        public void TestSingleIntensity2() {
            var data = new ApActivityData();

            Assert.False(data.IsSingleIntensitySpecified(new TimeSpan(0, 3, 30), 3));
        }

        [Fact]
        public void TestSingleIntensity3() {
            var data = new ApActivityData();

            data.Intensities[2] = "2256";

            Assert.False(data.IsSingleIntensitySpecified(new TimeSpan(0, 22, 56), 3));
        }

        [Fact]
        public void TestSingleIntensity4() {
            var data = new ApActivityData();

            data.Intensities[2] = "2256";

            Assert.False(data.IsSingleIntensitySpecified(new TimeSpan(0, 22, 57), 2));
        }

        [Fact]
        public void TestSingleIntensity5() {
            var data = new ApActivityData();

            data.Intensities[2] = "2256";

            Assert.True(data.IsSingleIntensitySpecified(new TimeSpan(0, 22, 56), 2));
        }
    }
}
