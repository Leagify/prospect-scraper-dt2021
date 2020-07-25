using CsvHelper;
using CsvHelper.Configuration;

namespace prospectScraper
{
    public sealed class MockDraftPickMap : ClassMap<MockDraftPick>
    {
        public MockDraftPickMap()
        {
            //AutoMap();
            // public int round;
            // public string teamCity;
            // public string pickNumber;
            // public string playerName;
            // public string school;
            // public string position;
            // public string reachValue;
            // public int leagifyPoints;
            // public string pickDate;
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