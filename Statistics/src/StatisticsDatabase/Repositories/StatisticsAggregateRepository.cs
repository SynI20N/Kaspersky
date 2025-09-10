using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using StatisticsDatabase.Context;
using StatisticsDatabase.Models;

namespace StatisticsDatabase.Repositories;

public class StatisticsAggregateRepository : CrudRepository<Statistic>, IAggregateRepository
{
    public StatisticsAggregateRepository(StatisticsDatabaseContext context) : base(context) { }

    private async Task<DbDataReader> ExecuteReaderAsync(string sql)
    {
        var conn = _context.Database.GetDbConnection();
        await conn.OpenAsync();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = sql;

        return await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
    }

    public Task<DbDataReader> AggregateBySeverityAndSort()
    {
        var sql = _context.Statistics
            .GroupBy(s => s.Severity)
            .Select(g => new { Severity = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .ToQueryString();

        return ExecuteReaderAsync(sql);
    }

    public Task<DbDataReader> AggregateByProductAndVersion()
    {
        var sql = _context.Statistics
            .GroupBy(s => new { s.Product, s.Version })
            .Select(g => new { g.Key.Product, g.Key.Version, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .ToQueryString();

        return ExecuteReaderAsync(sql);
    }

    public Task<DbDataReader> NoAggregate()
    {
        var sql = _context.Statistics
            .Select(s => new { s.TimeStamp, s.Severity, s.Product, s.Version, s.ErrorCode })
            .ToQueryString();

        return ExecuteReaderAsync(sql);
    }

    /// <summary>
    /// TOP 10 ErrorCodes by Product + Severity
    /// </summary>
    public Task<DbDataReader> AggregateTop10ErrorCodesByProductAndSeverity()
    {
        var sql = @"
            WITH group_errors AS (
                SELECT
                    [ErrorCode],
                    [Product],
                    [Severity],
                    COUNT(*) AS [Count]
                FROM [Statistics]
                GROUP BY [Product], [Severity], [ErrorCode]
            ),
            ranked AS (
                SELECT
                    [ErrorCode],
                    [Product],
                    [Severity],
                    [Count],
                    ROW_NUMBER() OVER (
                        PARTITION BY [Product], [Severity]
                        ORDER BY [Count] DESC
                    ) AS rn
                FROM group_errors
            )
            SELECT
                [ErrorCode],
                [Product],
                [Severity],
                [Count]
            FROM ranked
            WHERE rn <= 10;";

        return ExecuteReaderAsync(sql);
    }

    /// <summary>
    /// Почасовая агрегация Product + Version с наиболее частым ErrorCode
    /// </summary>
    public Task<DbDataReader> AggregateByHourProductVersionMaxErrorCode()
    {
        var sql = @"
            WITH error_counts AS (
                SELECT
                    DATEADD(hour, DATEDIFF(hour, 0, [TimeStamp]), 0) AS hour_bucket,
                    [Product],
                    [Version],
                    [ErrorCode],
                    COUNT(*) AS [Count]
                FROM [Statistics]
                GROUP BY DATEADD(hour, DATEDIFF(hour, 0, [TimeStamp]), 0), [Product], [Version], [ErrorCode]
            ),
            ranked AS (
                SELECT
                    hour_bucket,
                    [Product],
                    [Version],
                    [ErrorCode],
                    [Count],
                    ROW_NUMBER() OVER (
                        PARTITION BY hour_bucket, [Product], [Version]
                        ORDER BY [Count] DESC
                    ) AS rn
                FROM error_counts
            )
            SELECT
                hour_bucket,
                [Product],
                [Version],
                [ErrorCode],
                [Count]
            FROM ranked
            WHERE rn = 1;";

        return ExecuteReaderAsync(sql);
    }
}
