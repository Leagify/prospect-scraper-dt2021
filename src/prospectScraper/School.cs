namespace prospectScraper
{
    public class School
    {
        public string SchoolName { get; set; }
        public string Conference { get; set; }
        public string State { get; set; }

        public static string CheckSchool(string schoolName)
        {
            schoolName = schoolName switch
            {
                "Miami" => "Miami (FL)",
                "Mississippi" => "Ole Miss",
                "Central Florida" => "UCF",
                "MTSU" => "Middle Tennessee",
                "Eastern Carolina" => "East Carolina",
                "Pittsburgh" => "Pitt",
                "FIU" => "Florida International",
                "Florida St" => "Florida State",
                "Penn St" => "Penn State",
                "Minneosta" => "Minnesota",
                "Mississippi St" => "Mississippi State",
                "Mississippi St." => "Mississippi State",
                "Oklahoma St" => "Oklahoma State",
                "Boise St" => "Boise State",
                "Lenoir-Rhyne" => "Lenoir–Rhyne",
                "NCState" => "NC State",
                "W Michigan" => "Western Michigan",
                "UL Lafayette" => "Louisiana-Lafayette",
                "Cal" => "California",
                "S. Illinois" => "Southern Illinois",
                "UConn" => "Connecticut",
                "LA Tech" => "Louisiana Tech",
                "Louisiana" => "Louisiana-Lafayette",
                "San Diego St" => "San Diego State",
                "South Carolina St" => "South Carolina State",
                "Wake Forrest" => "Wake Forest",
                "NM State" => "New Mexico State",
                "New Mexico St" => "New Mexico State",
                "Southern Cal" => "USC",
                "x-USC" => "USC",
                "Mempis" => "Memphis",
                "Southeast Missouri" => "Southeast Missouri State",
                "Berry College" => "Berry",
                "USF" => "South Florida",
                "N Dakota State" => "North Dakota State",
                "SE Missouri State" => "Southeast Missouri State",
                "Appalachian St" => "Appalachian State",
                "N Illinois" => "Northern Illinois",
                "UL Monroe" => "Louisiana-Monroe",
                "Central Missouri St" => "Central Missouri",
                "North Carolina State" => "NC State",
                _ => schoolName,
            };
            return schoolName;
        }
    }
}
