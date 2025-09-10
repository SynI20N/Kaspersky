FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["./API/AlertDataProvider.csproj", "AlertService/AlertDataProvider.csproj"]
RUN dotnet restore "./AlertService/AlertDataProvider.csproj"
COPY ["./Workers/HostedServices.csproj", "Workers/HostedServices.csproj"]
RUN dotnet restore "./Workers/HostedServices.csproj"
WORKDIR "/src/Workers"
COPY ./Workers .
COPY --from=metricgateway.api /app/TrackSense.API.MetricGateway.dll Lib/
WORKDIR "/src/AlertService"
COPY ./API .
COPY --from=filldb /app/TimeScaleDB.dll Lib/
COPY --from=alertservices /app/HostedServices.dll Lib/
COPY --from=metricgateway.api /app/TrackSense.API.MetricGateway.dll Lib/
RUN dotnet build "AlertDataProvider.csproj" -c $BUILD_CONFIGURATION -o /app/build 

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "AlertDataProvider.csproj" -c $BUILD_CONFIGURATION -o /app/publish --self-contained -r linux-x64

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=publish /src/AlertService/appsettings.json /app/
ENTRYPOINT ["./AlertDataProvider"]