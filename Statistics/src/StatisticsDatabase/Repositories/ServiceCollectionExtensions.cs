using Microsoft.Extensions.DependencyInjection;
using StatisticsDatabase.Models;

namespace StatisticsDatabase.Repositories;

public static class ServiceCollectionExtensions
{
    public static void AddCrudRepositories(this IServiceCollection services)
    {
        services.AddTransient<ICrudRepository<Statistics>, CrudRepository<Statistics>>();
    }
}