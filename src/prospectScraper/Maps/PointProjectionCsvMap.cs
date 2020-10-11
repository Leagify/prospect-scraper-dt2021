using CsvHelper.Configuration;

namespace prospectScraper.Maps
{
    public sealed class PointProjectionCsvMap : ClassMap<PointProjection>
    {
        public PointProjectionCsvMap()
        {
            Map(m => m.Rank).Name("Rank");
            Map(m => m.ProjectedPoints).Name("Points");
        }
    }
}
