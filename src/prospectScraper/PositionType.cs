namespace prospectScraper
{
    public class PositionType
    {
        public string positionName;
        public string positionGroup;
        public string positionAspect;

        public PositionType () 
        {
        }

        public PositionType (string positionName, string positionGroup, string positionAspect)
        {
            this.positionName = positionName;
            this.positionGroup = positionGroup;
            this.positionAspect = positionAspect;
        }
    }
}
