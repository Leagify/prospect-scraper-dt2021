using prospectScraper;
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
            //Act
            var actual = Player.ConvertHeightToInches(heightInFt, "FOO");

            //Assert
            Assert.Equal(expectedHeightInInches, actual);
        }

        [Fact]
        public void Draft_Date_Defaults_To_Today()
        {
            //Arrange
            var expected = DateTime.Now.ToString("yyyy-MM-dd");

            //Act
            var actual = ProspectScraper.FormatDraftDate("TEST");

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Formats_Draft_Date()
        {
            //Arrange
            var expected = "2019-05-21";

            //Act
            var actual = ProspectScraper.FormatDraftDate(" May 21, 2019 2:00 AM EST");

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Invalid_Schools_Returns_Unmodified()
        {
            //Assert
            Assert.Equal("hogWarts", School.CheckSchool("hogWarts"));
        }

        [Theory]
        [InlineData("Miami", "Miami (FL)")]
        [InlineData("North Carolina State", "NC State")]
        public void Valid_Schools(string input, string expected)
        {
            //Act
            var actual = School.CheckSchool(input);

            //Assert
            Assert.Equal(expected, actual);
        }
    }
}
