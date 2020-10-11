using CsvHelper.Configuration;

namespace prospectScraper.Maps
{
    public sealed class ExistingProspectRankingCsvMap : ClassMap<ExistingProspectRanking>
    {
        public ExistingProspectRankingCsvMap()
        {
            Map(m => m.rank).Name("Rank");
            Map(m => m.change).Name("Change");
            Map(m => m.playerName).Name("Player");
            Map(m => m.school).Name("School");
            Map(m => m.position1).Name("Position");
            Map(m => m.height).Name("Height").ConvertUsing(map => Player.ConvertHeightToInches(map.GetField("Height"), map.GetField("Player")).ToString());
            Map(m => m.weight).Name("Weight");
            Map(m => m.collegeClass).Name("CollegeClass");
            Map(m => m.rankingDateString).Name("Date");
            Map(m => m.draftStatus).Name("DraftStatus");
        }
    }
}
