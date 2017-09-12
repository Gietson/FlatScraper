dotnet restore ../src/FlatScraper.API
dotnet restore ../src/FlatScraper.Cron
dotnet publish ../src/FlatScraper.API -c Release -o ./bin/Docker
dotnet publish ../src/FlatScraper.Cron -c Release -o ./bin/Docker