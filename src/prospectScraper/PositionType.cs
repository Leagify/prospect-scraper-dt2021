using CsvHelper.Configuration;

namespace prospect-scraper-dt2021
{
    public class PositionType
    {
        public string positionName;
        public string positionGroup;
        public string positionAspect;

        public PositionType () {}
        public PositionType (string positionName, string positionGroup, string positionAspect)
        {
            this.positionName = positionName;
            this.positionGroup = positionGroup;
            this.positionAspect = positionAspect;
        }
    }

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
