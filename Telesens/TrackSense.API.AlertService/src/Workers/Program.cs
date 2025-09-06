using TrackSense.API.AlertService.Workers;
using TrackSense.API.AlertService.Workers.Services;

var hostBuilder = Host.CreateApplicationBuilder(args);
hostBuilder.Services.AddSingleton<IConfiguration>(_ => hostBuilder.Configuration);
hostBuilder.Services.AddSingleton<IKafkaReceiver, KafkaReceiver>();
hostBuilder.Services.AddHostedService<AlertService>();
hostBuilder.Services.AddHostedService<AlertDataCrawler>();

var host = hostBuilder.Build();
host.Run();
