using CsvHelper.Configuration;

namespace prospectScraper.Maps
{
    public sealed class RegionCsvMap : ClassMap<Region>
    {
        public RegionCsvMap()
        {
            Map(m => m.State).Name("State");
            Map(m => m.RegionCode).Name("Region");
        }
    }
}
