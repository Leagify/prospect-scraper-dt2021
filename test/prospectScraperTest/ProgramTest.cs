using prospectScraper;
using System;
using Xunit;

namespace prospectScraperTest
{
    public class ProgramTest
    {
        private const string InvalidDateString = "TEST";

        [Theory]
        [InlineData(73, "6'1")]
        [InlineData(12, "1'0")]
        public void Converts_Height_To_Inches(int expectedHeightInInches, string heightInFt)
        {
            //Act
            int actual = Player.ConvertHeightToInches(heightInFt, "FOO");

            //Assert
            Assert.Equal(expectedHeightInInches, actual);
        }

        [Fact]
        public void Draft_Date_Defaults_To_Today()
        {
            //Arrange
            string expected = DateTime.Now.ToString("yyyy-MM-dd");

            string actual = ProspectScraper.ChangeDateStringToDateTime(InvalidDateString, true).ToString("yyyy-MM-dd");

            //Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("2019-05-21", "May 21, 2019 2:00 AM")]
        [InlineData("2020-01-01", "January 1, 2020 2:00 AM")]
        [InlineData("2010-12-25", "December 25, 2010 2:00 AM")]
        public void Formats_Draft_Date(string expected, string validDateString)
        {
            string actual = ProspectScraper.ChangeDateStringToDateTime(validDateString, true).ToString("yyyy-MM-dd");

            //Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("FakeSchool1")]
        [InlineData("Hogwarts")]
        [InlineData("MyAcademyWins")]
        public void Invalid_Schools_Returns_Unmodified(string invalidSchool)
        {
            //Assert
            Assert.Equal(invalidSchool, School.CheckSchool(invalidSchool));
        }

        [Theory]
        [InlineData("Miami", "Miami (FL)")]
        [InlineData("North Carolina State", "NC State")]
        public void Valid_Schools(string input, string expected)
        {
            //Act
            string actual = School.CheckSchool(input);

            //Assert
            Assert.Equal(expected, actual);
        }
    }
}
