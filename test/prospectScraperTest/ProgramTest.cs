using System;
using Xunit;

namespace prospectScraperTest
{
    public class ProgramTest
    {
        [Theory]
        [InlineData(73, "6'1")]
        [InlineData(12, "1'0")]
        public void Converts_Height_To_Inches(int expectedHeightInInches, string heightInFt)
        {
            var actual = prospectScraper.Program.ConvertHeightToInches(heightInFt, "FOO");

            Assert.Equal(expectedHeightInInches, actual);
        }

        [Fact]
        public void Draft_Date_Defaults_To_Today()
        {
            var expected = DateTime.Now.ToString("yyyy-MM-dd");

            var actual = prospectScraper.Program.FormatScrapedDate("TEST");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Formats_Draft_Date()
        {
            var expected = "2019-05-21";

            var actual = prospectScraper.Program.FormatScrapedDate(" May 21, 2019 2:00 AM EST");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Invalid_Schools_Returns_Unmodified()
        {
            Assert.Equal("hogWarts", prospectScraper.Program.CheckSchool("hogWarts"));
        }

        [Theory]
        [InlineData("Miami", "Miami (FL)")]
        [InlineData("North Carolina State", "NC State")]
        public void Valid_Schools(string input, string expected)
        {
            var actual = prospectScraper.Program.CheckSchool(input);

            Assert.Equal(expected, actual);
        }
    }
}
