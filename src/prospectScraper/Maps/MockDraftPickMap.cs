using CsvHelper.Configuration;

namespace prospectScraper.Maps
{
    public sealed class MockDraftPickMap : ClassMap<MockDraftPick>
    {
        public MockDraftPickMap()
        {
            Map(m => m.pickNumber).Index(0).Name("Pick");
            Map(m => m.round).Index(1).Name("Round");
            Map(m => m.playerName).Index(2).Name("Player");
            Map(m => m.school).Index(3).Name("School");
            Map(m => m.position).Index(4).Name("Position");
            Map(m => m.teamCity).Index(5).Name("Team");
            Map(m => m.reachValue).Index(6).Name("ReachValue");
            Map(m => m.leagifyPoints).Index(7).Name("Points");
            Map(m => m.pickDate).Index(8).Name("Date");
            Map(m => m.state).Index(9).Name("State");
        }
    }
}