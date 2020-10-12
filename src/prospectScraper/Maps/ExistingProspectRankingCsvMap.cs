using CsvHelper.Configuration;

namespace prospectScraper.Maps
{
    public sealed class ExistingProspectRankingCsvMap : ClassMap<ExistingProspectRanking>
    {
        public ExistingProspectRankingCsvMap()
        {
            Map(m => m.Rank).Name("Rank");
            Map(m => m.Change).Name("Change");
            Map(m => m.PlayerName).Name("Player");
            Map(m => m.School).Name("School");
            Map(m => m.Position1).Name("Position");
            Map(m => m.Height).Name("Height").ConvertUsing(map => Player.ConvertHeightToInches(map.GetField("Height"), map.GetField("Player")).ToString());
            Map(m => m.Weight).Name("Weight");
            Map(m => m.CollegeClass).Name("CollegeClass");
            Map(m => m.RankingDateString).Name("Date");
            Map(m => m.DraftStatus).Name("DraftStatus");
        }
    }
}
