using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace StatisticsDatabase.Context;

public class DesignTimeContextFactory : IDesignTimeDbContextFactory<StatisticsDatabaseContext>
{
    public StatisticsDatabaseContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .AddUserSecrets<Program>()
            .Build();

        return new StatisticsDatabaseContext(config);
    }
}
