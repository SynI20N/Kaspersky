using TrackSense.API.AlertService.Models;

namespace TrackSense.API.AlertService.Services;
public static class OperatorMaps
{
    public static readonly Dictionary<Operator, Func<float, float, bool>> FuncMappings = new Dictionary<Operator, Func<float, float, bool>>{
        {Operator.Equal, (f1, f2) => f1 == f2},
        {Operator.LessThan, (f1, f2) => f1 < f2},
        {Operator.LessThanOrEqual, (f1, f2) => f1 <= f2},
        {Operator.MoreThan, (f1, f2) => f1 > f2},
        {Operator.MoreThanOrEqual, (f1, f2) => f1 >= f2}
    };

    public static readonly Dictionary<Operator, string> StringMappings = new Dictionary<Operator, string>{
        {Operator.Equal, "=="},
        {Operator.LessThan, "<"},
        {Operator.LessThanOrEqual, "<="},
        {Operator.MoreThan, ">"},
        {Operator.MoreThanOrEqual, ">="}
    };
}