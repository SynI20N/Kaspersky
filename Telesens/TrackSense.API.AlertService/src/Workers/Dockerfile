FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["HostedServices.csproj", "HostedServices/HostedServices.csproj"]
RUN dotnet restore "./HostedServices/HostedServices.csproj"
WORKDIR "/src/HostedServices"
COPY . .
COPY --from=metricgateway.api /app/TrackSense.API.MetricGateway.dll Lib/
RUN dotnet build "HostedServices.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "HostedServices.csproj" -c $BUILD_CONFIGURATION -o /app/publish --self-contained -r linux-x64

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=publish /src/HostedServices/appsettings.json /app/
ENTRYPOINT ["./HostedServices"]