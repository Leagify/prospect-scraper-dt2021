using CsvHelper.Configuration;

namespace prospectScraper.Maps
{
    public sealed class ProspectRankSimpleCsvMap : ClassMap<ProspectRankSimple>
    {
        public ProspectRankSimpleCsvMap()
        {
            Map(m => m.rank).Name("Rank");
            Map(m => m.playerName).Name("Player");
            Map(m => m.school).Name("School");
            Map(m => m.rankingDateString).Name("Date");
        }
    }
}
