using Xunit;
using prospectScraper;

namespace scrapysharpTest
{
    public class MockDraftPickTests
    {
        private const string StateFullName = "Florida";
        private const string CityStateAbr = "Miami (FL)";

        [Fact(Skip="Work needs to be done to automatically create/copy csv to specific directory.")]
        public void GetState_Returns_Correct_State_From_School()
        {
            //Assert
            Assert.Equal(StateFullName, MockDraftPick.GetState(CityStateAbr));
        }

        [Theory]
        [InlineData("1", 1)]
        [InlineData("33", 2)]
        [InlineData("65", 3)]
        [InlineData("104", 4)]
        [InlineData("147", 5)]
        [InlineData("215", 7)]
        public void ConvertPickToRound_Returns_Correct_Round(string pick, int expectedRoundValue)
        {
            //Assert
            Assert.Equal(expectedRoundValue, MockDraftPick.ConvertPickToRound(pick));
        }

        [Theory]
        [InlineData("1", 0, 40)]
        [InlineData("2", 0, 35)]
        [InlineData("11", 0, 30)]
        [InlineData("21", 0, 25)]
        [InlineData("33", 0, 20)]
        [InlineData("49", 0, 15)]
        [InlineData("0", 3, 10)]
        [InlineData("0", 4, 8)]
        [InlineData("0", 5, 7)]
        [InlineData("0", 6, 6)]
        [InlineData("0", 7, 5)]
        public void ConvertPickToPoints_Returns_Correct_Points(string pick, int round, int expectedPointsValue)
        {
            //Assert
            Assert.Equal(expectedPointsValue, MockDraftPick.ConvertPickToPoints(pick, round));
        }
    }
}
