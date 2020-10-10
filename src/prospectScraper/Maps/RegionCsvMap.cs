using CsvHelper.Configuration;

namespace prospectScraper.Maps
{
    public sealed class RegionCsvMap : ClassMap<Region>
    {
        public RegionCsvMap()
        {
            Map(m => m.state).Name("State");
            Map(m => m.region).Name("Region");
        }
    }
}
