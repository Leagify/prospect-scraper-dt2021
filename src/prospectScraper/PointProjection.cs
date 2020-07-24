using CsvHelper.Configuration;

namespace prospect-scraper-dt2021
{
    public class PointProjection
    {
        public string rank;
        public string projectedPoints;

        public PointProjection () {}
        public PointProjection (string rank, string projectedPoints)
        {
            this.rank = rank;
            this.projectedPoints = projectedPoints;
        }
    }

    public sealed class PointProjectionCsvMap : ClassMap<PointProjection>
    {
        public PointProjectionCsvMap()
        {
            Map(m => m.rank).Name("Rank");
            Map(m => m.projectedPoints).Name("Points");
        }
    }
}
