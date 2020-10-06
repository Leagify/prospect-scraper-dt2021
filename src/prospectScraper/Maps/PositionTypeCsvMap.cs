using CsvHelper.Configuration;

namespace prospectScraper.Maps
{
    public sealed class PositionTypeCsvMap : ClassMap<PositionType>
    {
        public PositionTypeCsvMap()
        {
            Map(m => m.positionName).Name("Pos");
            Map(m => m.positionGroup).Name("Group");
            Map(m => m.positionAspect).Name("Type");
        }
    }
}
