using System;
using System.Threading.Tasks;

namespace prospectScraper
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var prospectScraper = new ProspectScraper();
            bool parseDate = true;
            // Initial attempt to handle command line arguments.
            // "b" for the big boards, "m" for mock drafts, "e" for everything
            if (args.Length == 0)
            {
                Console.WriteLine(
                    "No Arguments provided - Type \"bb\" for big board, \"md\" for mock draft, \"all\" for both. Running both by default...");
                await prospectScraper.RunTheBigBoardsAsync();
                Console.WriteLine(
                    "No Arguments- Type bb for big board, md for mock draft, all for both. Running both by default.....");
                await prospectScraper.RunTheBigBoardsAsync();
            }
            else
            {
                string context = args[0].ToLower();
                if (args.Length >= 2)
                {
                    string ignoreDate = args[1].ToLower();
                    if (ignoreDate == "ignoredate")
                    {
                        parseDate = false;
                    }
                }

                switch (context)
                {
                    case "bb":
                        Console.WriteLine("Running Big Board");
                        await prospectScraper.RunTheBigBoardsAsync(parseDate);
                        break;
                    case "md":
                        Console.WriteLine("Running Mock Draft");
                        await prospectScraper.RunTheMockDraft(parseDate);
                        break;
                    case "all":
                        Console.WriteLine("Running Big Board and Mock Draft");
                        await prospectScraper.RunTheBigBoardsAsync(parseDate);
                        await prospectScraper.RunTheMockDraft(parseDate);
                        break;
                    default:
                        Console.WriteLine("Input argument of " + context + " not recognized.  Please try running again.");
                        await prospectScraper.RunTheBigBoardsAsync(parseDate);
                        break;
                }
            }
        }
    }
}