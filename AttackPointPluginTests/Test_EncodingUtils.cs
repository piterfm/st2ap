using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using GK.AttackPoint;

namespace AttackPointPluginTests
{
    // The test is very inconclusive
    public class Test_EncodingUtils
    {
        [Fact]
        public void PlainLatin() {
            var input = "The quick brown fox jumps over the lazy dog";
            Assert.Equal(input, EncodingUtils.ConvertForLatin1Html(input));
        }

        [Fact]
        public void SomeSpanish() {
            var input = "aprobación este año";
            Assert.Equal(input, EncodingUtils.ConvertForLatin1Html(input));
        }

        [Fact]
        public void SomeNorwegianCharacters() {
            var input = "æøå";
            Assert.Equal(input, EncodingUtils.ConvertForLatin1Html(input));
        }

        [Fact]
        public void SomeSwedish() {
            var input = "Tänk efter nu förr'n vi föser dig bort";
            Assert.Equal(input, EncodingUtils.ConvertForLatin1Html(input));
        }

        [Fact]
        public void SomeRussian() {
            var input = "Ориентирование - это круто";
            Assert.Equal("&#1054;&#1088;&#1080;&#1077;&#1085;&#1090;&#1080;&#1088;&#1086;&#1074;&#1072;&#1085;&#1080;&#1077; - &#1101;&#1090;&#1086; &#1082;&#1088;&#1091;&#1090;&#1086;",
                EncodingUtils.ConvertForLatin1Html(input));
        }

        [Fact]
        public void SomeSpecialCharacters() {
            var input = "–€";
            Assert.Equal("&#8211;&#8364;", EncodingUtils.ConvertForLatin1Html(input));
        }
    }
}
