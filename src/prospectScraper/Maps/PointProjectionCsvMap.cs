using CsvHelper.Configuration;

namespace prospectScraper.Maps
{
    public sealed class PointProjectionCsvMap : ClassMap<PointProjection>
    {
        public PointProjectionCsvMap()
        {
            Map(m => m.rank).Name("Rank");
            Map(m => m.projectedPoints).Name("Points");
        }
    }
}
