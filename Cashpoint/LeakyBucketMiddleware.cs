using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using System.Net;

namespace RateLimitingMiddleware
{
    public class LeakyBucketOptions
    {
        public int BucketSize { get; set; } = 100; // Размер ведра
        public int ProcessingRate { get; set; } = 1; // Колличество обрабатываемых запросов в секунду
        public int Timeout { get; set; } = 1000; // Время между проверками в миллисекундах
    }

    public class LeakyBucketMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly LeakyBucketOptions _options;
        private readonly ConcurrentDictionary<IPAddress, int> _buckets = new();

        public LeakyBucketMiddleware(RequestDelegate next, IOptions<LeakyBucketOptions> options)
        {
            _next = next;
            _options = options.Value;

            // Создаем таймер для опустошения ведер
            var timer = new System.Timers.Timer(_options.Timeout);
            timer.Elapsed += (sender, e) => ProcessRequests();
            timer.Start();
        }

        public async Task Invoke(HttpContext context)
        {
            // Получаем IP-адрес клиента
            var clientIp = context.Connection.RemoteIpAddress;

            // Получаем или создаем ведро для данного IP
            if (!_buckets.TryGetValue(clientIp, out int tokens))
            {
                tokens = 0;
                _buckets.TryAdd(clientIp, tokens);
            }

            // Пытаемся добавить запрос в ведро
            if (Interlocked.Increment(ref tokens) > _options.BucketSize)
            {
                // Если ведро полное - возвращаем 429 Too Many Requests
                context.Response.StatusCode = 429;
                await context.Response.WriteAsync("Too Many Requests");
                // Можно добавить пенальти которое дополнительно переполнит ведро и не даст пользователю пользоваться сервисом дополнительное время
                // _buckets[clientIp] = Interlocked.Add(ref tokens, 15);
                return;
            }
            _buckets[clientIp] = Interlocked.Increment(ref tokens);
            // Пропускаем запрос дальше
            await _next(context);
        }

        private void ProcessRequests()
        {
            for (int i = 0; i < _options.ProcessingRate; i++)
                foreach (var ips in _buckets.Keys)
                    if (_buckets[ips] > 0)
                    {
                        var value = _buckets[ips];
                        Interlocked.Decrement(ref value);
                        _buckets[ips] = value;
                    }
        }

    }

    public static class LeakyBucketMiddlewareExtensions
    {
        public static IApplicationBuilder UseLeakyBucket(this IApplicationBuilder builder) =>
            builder.UseMiddleware<LeakyBucketMiddleware>();
    }
}
