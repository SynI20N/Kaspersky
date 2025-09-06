using Microsoft.EntityFrameworkCore;
using TrackSense.API.AlertService;
using TrackSense.API.AlertService.Models;

public class RulesRepository : IRulesRepository
{
    private readonly AlertServiceContext _context;
    private readonly DbSet<AlertRule> _rules;

    public RulesRepository(AlertServiceContext context)
    {
        _context = context;
        _rules = _context.Set<AlertRule>();
    }
    public async Task<List<AlertRule>> GetAlertRulesByImeiAsync(long imei)
    {
        return await _rules.Where(r => r.Imei == imei).ToListAsync();
    }
}