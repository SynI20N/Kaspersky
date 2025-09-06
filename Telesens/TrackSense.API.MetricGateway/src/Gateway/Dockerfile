FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TrackSense.API.MetricGateway.csproj", "TrackSense.API.MetricGateway/"]
RUN dotnet restore "./TrackSense.API.MetricGateway/TrackSense.API.MetricGateway.csproj"
WORKDIR "/src/TrackSense.API.MetricGateway"
COPY . .
RUN dotnet build "./TrackSense.API.MetricGateway.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TrackSense.API.MetricGateway.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=publish /src/TrackSense.API.MetricGateway/appsettings.json /app/
ENTRYPOINT ["dotnet", "TrackSense.API.MetricGateway.dll"]