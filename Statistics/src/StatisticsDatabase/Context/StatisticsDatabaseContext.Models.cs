using Microsoft.EntityFrameworkCore;
using StatisticsDatabase.Models;

namespace StatisticsDatabase.Context;

public partial class StatisticsDatabaseContext : DbContext
{
    public DbSet<Statistic> Statistics => Set<Statistic>();
}
