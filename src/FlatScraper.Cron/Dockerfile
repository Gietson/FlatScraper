FROM microsoft/dotnet:2.1-aspnetcore-runtime
WORKDIR /dotnetapp
COPY ./bin/Docker .
ENV ASPNETCORE_URLS http://*:5050
ENV ASPNETCORE_ENVIRONMENT docker
ENTRYPOINT dotnet FlatScraper.Cron.dll