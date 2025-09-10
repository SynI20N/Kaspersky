using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;
using Npgsql;
using TrackSense.API.AlertService.Models;
using System.Data;

namespace TrackSense.API.AlertService;

public static class DbSetExtensions
{
    public static EntityEntry<T> AddIfNotExists<T>(this DbSet<T> dbSet, T entity, Expression<Func<T, bool>> predicate = null) where T : class, new()
    {
        var exists = predicate != null ? dbSet.Any(predicate) : dbSet.Any();
        return !exists ? dbSet.Add(entity) : null;
    }
}

public partial class AlertServiceContext : DbContext
{
    private readonly IConfiguration _configuration;

    public AlertServiceContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AlertServiceContext).Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured)
        {
            return;
        }
        optionsBuilder
            .UseLazyLoadingProxies()
            .UseNpgsql(_configuration.GetConnectionString("DBConnection"));
    }
}