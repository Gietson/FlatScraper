FROM microsoft/aspnetcore:2.0
WORKDIR /dotnetapp
COPY out .
ENV ASPNETCORE_URLS http://*:5000
ENV ASPNETCORE_ENVIRONMENT docker
ENTRYPOINT ["dotnet", "FlatScraper.API.dll"]