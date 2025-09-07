using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace StatisticsDatabase.Context;

public partial class StatisticsDatabaseContext : DbContext
{
    private readonly IConfiguration _configuration;

    public StatisticsDatabaseContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(StatisticsDatabaseContext).Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured)
        {
            return;
        }
        optionsBuilder.UseSqlServer(_configuration.GetConnectionString("StatisticsDatabase"));
    }
}