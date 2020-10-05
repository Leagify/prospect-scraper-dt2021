using CsvHelper.Configuration;

namespace prospectScraper.Maps
{
    public sealed class SchoolCsvMap : ClassMap<School>
    {
        public SchoolCsvMap()
        {
            Map(m => m.schoolName).Name("School");
            Map(m => m.conference).Name("Conference");
            Map(m => m.state).Name("State");
        }
    }
}
