# prospectScraper

This is a webscraper that uses [ScrapySharp](https://github.com/rflechner/ScrapySharp) to obtain american football draft prospect information.

Output is stored as a CSV file, made with the help of LINQ and [CSVHelper](https://joshclose.github.io/CsvHelper/).

Some limited error checking is included to verify whether the school matches up to a pre-existing list of schools and the states they are found.

This program is written in .NET 5.

For the previous year, this project existed in two separate projects, one for the [big board](https://github.com/Leagify/scrapysharp-dt2020), and one for the [mock draft](https://github.com/Leagify/mockdraft-2020). Part of these projects were made possible by the help of Hacktoberfest.


[![Open in Gitpod](https://gitpod.io/button/open-in-gitpod.svg)](https://gitpod.io#https://github.com/Leagify/prospect-scraper-dt2021
)

Note- I assume that you are going to run this in GitPod (which runs Ubuntu 18.04), so it uses the `linux-x64` by default.  If you're in Windows, make sure to use the `-r win-x64` parameter when building or running, since the .csproj file has a linux runtime set by default.

### Running the application
- Go to the `prospect-scraper-dt2021/src/prospectScraper` directory.
- Type `dotnet run` and add one of three parameters: 
  + `bb` runs the big board scraper
  + `md` runs the mock draft scraper 
  + `all` runs both big board and mock draft scrapers
  + when in doubt, do `dotnet run all`
  
#### Running tests
There is rudimentary support for tests built in.
- To run tests, go to the `prospect-scraper-dt2021/test/prospectScraperTest` directory
- Type `dotnet test` and watch the test(s) run.
- For more info, check out the [docs](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-test).
