using CsvHelper.Configuration;

namespace prospectScraper.Maps
{
    public sealed class MockDraftPickMap : ClassMap<MockDraftPick>
    {
        public MockDraftPickMap()
        {
            Map(m => m.PickNumber).Index(0).Name("Pick");
            Map(m => m.Round).Index(1).Name("Round");
            Map(m => m.PlayerName).Index(2).Name("Player");
            Map(m => m.School).Index(3).Name("School");
            Map(m => m.Position).Index(4).Name("Position");
            Map(m => m.TeamCity).Index(5).Name("Team");
            Map(m => m.ReachValue).Index(6).Name("ReachValue");
            Map(m => m.LeagifyPoints).Index(7).Name("Points");
            Map(m => m.PickDate).Index(8).Name("Date");
            Map(m => m.State).Index(9).Name("State");
        }
    }
}