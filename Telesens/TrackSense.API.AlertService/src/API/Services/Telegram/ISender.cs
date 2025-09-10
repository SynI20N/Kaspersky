namespace TrackSense.API.AlertService.Services;

public interface ISender 
{
    public Task<bool> SendAsync(string id, string message);
    public Task SendAllAsync();
}