using Microsoft.EntityFrameworkCore;
using TrackSense.API.AlertService;
using TrackSense.API.AlertService.Models;

public class EventsRepository : IEventsRepository
{
    private readonly AlertServiceContext _context;
    private readonly DbSet<AlertEvent> _events;

    public EventsRepository(AlertServiceContext context)
    {
        _context = context;
        _events = _context.Set<AlertEvent>();
    }

    public async Task<bool> ChangeStatusByRuleAndImeiAsync(int AlertRuleId, long Imei, AlertEventStatus newStatus)
    {
        var e = await _events.Where(e => e.AlertRuleId == AlertRuleId && e.IMEI == Imei).FirstOrDefaultAsync();

        if(e == null)
            return false;

        e.Status = newStatus;
        return await UpdateEventAsync(e);
    }

    public async Task<bool> EventExistsAsync(int AlertRuleId, long Imei)
    {
        var e = await _events.Where(e => e.AlertRuleId == AlertRuleId && e.IMEI == Imei).FirstOrDefaultAsync();

        return e == null ? false : true;
    }

    public async Task<AlertEventStatus> GetStatusByRuleAndImeiAsync(int AlertRuleId, long Imei)
    {
        return await _events.Where(e => e.AlertRuleId == AlertRuleId && e.IMEI == Imei).Select(e => e.Status).FirstOrDefaultAsync();
    }

    public async Task<AlertEvent?> GetByRuleAndImeiAsync(int AlertRuleId, long Imei)
    {
        return await _events.Where(e => e.AlertRuleId == AlertRuleId && e.IMEI == Imei).FirstOrDefaultAsync();
    }

    public async Task<bool> InsertEventAsync(AlertEvent alertEvent)
    {
        await _events.AddAsync(alertEvent);
        var inserted = await _context.SaveChangesAsync();
        return inserted > 0 ? true : false;
    }

    public async Task<bool> UpdateEventAsync(AlertEvent alertEvent)
    {
        _context.Entry(alertEvent).State = EntityState.Modified;
        var updated = await _context.SaveChangesAsync();
        return updated > 0 ? true : false;
    }
}