using System.Data.Common;

namespace StatisticsDatabase.Repositories;

public interface IAggregateRepository
{
    public Task<DbDataReader> NoAggregate();
    public Task<DbDataReader> AggregateBySeverityAndSort();
    public Task<DbDataReader> AggregateByProductAndVersion();
    public Task<DbDataReader> AggregateTop10ErrorCodesByProductAndSeverity();
    public Task<DbDataReader> AggregateByHourProductVersionMaxErrorCode();

}
