FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Cashpoint.csproj", "Cashpoint/"]
RUN dotnet restore "./Cashpoint/Cashpoint.csproj"
WORKDIR "/src/Cashpoint"
COPY . .
RUN dotnet build "./Cashpoint.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Cashpoint.csproj" -c $BUILD_CONFIGURATION -o /app/publish --self-contained

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["./Cashpoint"]