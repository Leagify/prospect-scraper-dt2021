namespace prospectScraper
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var prospectScraper = new ProspectScraper();
            bool parseDate = true;
            // Initial attempt to handle command line arguments.
            // "b" for the big boards, "m" for mock drafts, "e" for everything
            if (args.Length == 0)
            {
                Console.WriteLine(
                    "No Arguments provided - Type \"bb\" for big board, \"md\" for mock draft, \"all\" for both. Running both by default...");
                prospectScraper.RunTheBigBoards();
                Console.WriteLine(
                    "No Arguments- Type bb for big board, md for mock draft, all for both. Running both by default.....");
                RunTheBigBoards(parseDate);
            }
            else
            {
                string context = args[0].ToString().ToLower();
                if (args.Length >= 2)
                {
                    string ignoreDate = args[1].ToString().ToLower();
                    if (ignoreDate == "ignoredate")
                    {
                        parseDate = false;
                    }
                }

                switch (context)
                {
                    case "bb":
                        Console.WriteLine("Running Big Board");
                        prospectScraper.RunTheBigBoards(parseDate);
                        prospectScraper.RunTheBigBoards(parseDate);
                        break;
                    case "md":
                        Console.WriteLine("Running Mock Draft");
                        prospectScraper.RunTheMockDraft(parseDate);
                        prospectScraper.RunTheMockDraft(parseDate);
                        break;
                    case "all":
                        Console.WriteLine("Running Big Board and Mock Draft");
                        prospectScraper.RunTheBigBoards(parseDate);
                        prospectScraper.RunTheMockDraft(parseDate);
                        break;
                    default:
                        Console.WriteLine("Input argument of " + s + " not recognized. Please try running again.");
                        Console.WriteLine(
                            "Input argument of " + context + " not recognized.  Please try running again.");
                        prospectScraper.RunTheBigBoards(parseDate);
                        break;
                }
            }
        }
    }
}