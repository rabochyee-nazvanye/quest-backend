FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY . .
RUN dotnet restore && dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY appsettings.json .
RUN curl -ks 'https://storage.yandexcloud.net/cloud-certs/CA.pem' -o '/usr/local/share/ca-certificates/YCloudRootCA.crt' && chmod 644 '/usr/local/share/ca-certificates/YCloudRootCA.crt' && update-ca-certificates

ENTRYPOINT ["dotnet", "Quest.API.dll"]

