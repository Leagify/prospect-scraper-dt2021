using CsvHelper.Configuration;

namespace prospectScraper.Maps
{
    public sealed class MockDraftPickCsvMap : ClassMap<MockDraftPick>
    {
        public MockDraftPickCsvMap()
        {
            //Pick,Round,Player,School,Position,Team,ReachValue,Points,Date
            Map(m => m.PickNumber).Name("Pick");
            Map(m => m.Round).Name("Round");
            Map(m => m.PlayerName).Name("Player");
            Map(m => m.School).Name("School");
            Map(m => m.Position).Name("Position");
            Map(m => m.TeamCity).Name("Team");
            Map(m => m.ReachValue).Name($"ReachValue");
            Map(m => m.LeagifyPoints).Name("Points");
            Map(m => m.PickDate).Name("Date");
            Map(m => m.State).Name("State");
        }
    }
}
