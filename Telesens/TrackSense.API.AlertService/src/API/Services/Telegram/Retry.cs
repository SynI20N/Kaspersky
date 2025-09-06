namespace TrackSense.API.AlertService.Services;

public struct Retry
{
    public List<Func<Task<bool>>> Funcs { get; }
    public int Count { get; private set; }

    public Retry(Func<Task<bool>> someFunc, int count)
    {
        Funcs = [someFunc];
        Count = count;
    }

    public Retry(List<Func<Task<bool>>> funcs, int count)
    {
        Funcs = funcs;
        Count = count;
    }

    public void AddRetryFunction(Func<Task<bool>> someFunc)
    {
        if(someFunc == null)
        {
            return;
        }
        Funcs.Add(someFunc);
    }
}