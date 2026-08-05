# Orquestrador CT-e — BFF (ASP.NET Core 8)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY Orquestrador.Api/src/Orquestrador.Domain/ Orquestrador.Domain/
COPY Orquestrador.Api/src/Orquestrador.Application/ Orquestrador.Application/
COPY Orquestrador.Api/src/Orquestrador.Infrastructure/ Orquestrador.Infrastructure/
COPY Orquestrador.Api/src/Orquestrador.Api/ Orquestrador.Api/
RUN dotnet restore Orquestrador.Api/Orquestrador.Api.csproj
RUN dotnet publish Orquestrador.Api/Orquestrador.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
RUN apt-get update \
    && apt-get install -y --no-install-recommends curl \
    && rm -rf /var/lib/apt/lists/*
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 5000
ENTRYPOINT ["dotnet", "Orquestrador.Api.dll"]
