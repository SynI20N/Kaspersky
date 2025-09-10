FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY . .
RUN apt-get update && \
    apt-get install -y p7zip-full && \
    rm -rf /var/lib/apt/lists/*
WORKDIR /app/ZipperAPI.Tests
RUN dotnet restore "ZipperAPI.Tests.csproj"
RUN dotnet build "ZipperAPI.Tests.csproj" -c Release -o /app/build
CMD ["dotnet", "test", "ZipperAPI.Tests.csproj", "--logger:trx", "--results-directory", "/app/test-results"]
