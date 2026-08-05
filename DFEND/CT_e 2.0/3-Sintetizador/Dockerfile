# Monitor Sintetizador CT-e — BFF (ASP.NET Core 8)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY Sintetizador.Api/src/ ./
RUN dotnet restore Monitor.Api/Monitor.Api.csproj
RUN dotnet publish Monitor.Api/Monitor.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
RUN apt-get update \
    && apt-get install -y --no-install-recommends curl \
    && rm -rf /var/lib/apt/lists/*
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:5030
ENV ASPNETCORE_ENVIRONMENT=Development
EXPOSE 5030
ENTRYPOINT ["dotnet", "Monitor.Api.dll"]
