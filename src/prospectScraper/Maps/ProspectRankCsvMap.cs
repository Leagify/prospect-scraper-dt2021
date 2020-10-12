using CsvHelper.Configuration;

namespace prospectScraper.Maps
{
    public sealed class ProspectRankSimpleCsvMap : ClassMap<ProspectRankSimple>
    {
        public ProspectRankSimpleCsvMap()
        {
            Map(m => m.Rank).Name("Rank");
            Map(m => m.PlayerName).Name("Player");
            Map(m => m.School).Name("School");
            Map(m => m.RankingDateString).Name("Date");
        }
    }
}
