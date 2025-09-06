FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS bundle
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["./Database/TimeScaleDB.csproj", "Database/TimeScaleDB.csproj"]
RUN dotnet restore "./Database/TimeScaleDB.csproj"
COPY ["./API/AlertDataProvider.csproj", "API/AlertDataProvider.csproj"]
RUN dotnet restore "./API/AlertDataProvider.csproj"
COPY ["./Workers/HostedServices.csproj", "Workers/HostedServices.csproj"]
RUN dotnet restore "./Workers/HostedServices.csproj"
WORKDIR /src/API
COPY ./API .
COPY --from=metricgateway.api /app/TrackSense.API.MetricGateway.dll Lib/
WORKDIR /src/Database
COPY ./Database .
WORKDIR /src/Workers
COPY ./Workers .
COPY --from=metricgateway.api /app/TrackSense.API.MetricGateway.dll Lib/
WORKDIR /src
RUN dotnet tool install dotnet-ef --tool-path "."
RUN ./dotnet-ef migrations bundle -p Database -s API --self-contained -r linux-x64 -v

FROM bundle AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR /src/Database
RUN dotnet publish "TimeScaleDB.csproj" -c $BUILD_CONFIGURATION -o /app/publish --self-contained -r linux-x64

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
WORKDIR /src
COPY --from=bundle /src/API/appsettings* .
COPY --from=bundle /src .
ENTRYPOINT ["./efbundle"]