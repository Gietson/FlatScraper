#!/bin/bash
dotnet publish ./src/FlatScraper.API -c Release -o ./bin/Docker
dotnet publish ./src/FlatScraper.Cron -c Release -o ./bin/Docker