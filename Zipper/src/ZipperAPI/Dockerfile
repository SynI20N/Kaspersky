FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY . .
RUN apt-get update && \
    apt-get install -y p7zip-full && \
    rm -rf /var/lib/apt/lists/*
RUN dotnet restore "ZipperAPI.csproj"
RUN dotnet build "ZipperAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ZipperAPI.csproj" -c Release -o /app/publish

FROM base AS final
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ZipperAPI.dll"]
