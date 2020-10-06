using System;

namespace prospectScraper
{
    public class Program
    {
        static void Main(string[] args)
        {
            var ProspectScraper = new ProspectScraper();
            // Initial attempt to handle command line arguments.
            // "b" for the big boards, "m" for mock drafts, "e" for everything
            if (args.Length == 0)
            {
                Console.WriteLine("No Arguments provided - Type \"bb\" for big board, \"md\" for mock draft, \"all\" for both. Running both by default...");
                ProspectScraper.RunTheBigBoards();
            }
            else
            {
                string s = args[0].ToString().ToLower();
                switch (s)
                {
                    case "bb":
                        Console.WriteLine("Running Big Board");
                        ProspectScraper.RunTheBigBoards();
                        break;
                    case "md":
                        Console.WriteLine("Running Mock Draft");
                        ProspectScraper.RunTheMockDraft();
                        break;
                    case "all":
                        Console.WriteLine("Running Big Board and Mock Draft");
                        ProspectScraper.RunTheBigBoards();
                        ProspectScraper.RunTheMockDraft();
                        break;
                    default:
                        Console.WriteLine("Input argument of " + s + " not recognized. Please try running again.");
                        break;
                }
            }
        }
    }
}