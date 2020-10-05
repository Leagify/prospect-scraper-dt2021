namespace prospectScraper
{
    public class Region
	{
		public string state;
		public string region;

		public Region () { }
		public Region (string state, string region)
		{
			this.region = region;
			this.state = state;
		}
	}
}
