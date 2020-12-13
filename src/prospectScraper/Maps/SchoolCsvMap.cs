using CsvHelper.Configuration;

namespace prospectScraper.Maps
{
    public sealed class SchoolCsvMap : ClassMap<School>
    {
        public SchoolCsvMap()
        {
            Map(m => m.SchoolName).Name("School");
            Map(m => m.Conference).Name("Conference");
            Map(m => m.State).Name("State");
        }
    }
}
