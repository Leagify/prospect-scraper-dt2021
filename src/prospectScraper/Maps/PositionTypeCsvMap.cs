using CsvHelper.Configuration;

namespace prospectScraper.Maps
{
    public sealed class PositionTypeCsvMap : ClassMap<PositionType>
    {
        public PositionTypeCsvMap()
        {
            Map(m => m.PositionName).Name("Pos");
            Map(m => m.PositionGroup).Name("Group");
            Map(m => m.PositionAspect).Name("Type");
        }
    }
}
