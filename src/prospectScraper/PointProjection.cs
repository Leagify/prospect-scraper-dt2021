namespace prospectScraper
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


}
