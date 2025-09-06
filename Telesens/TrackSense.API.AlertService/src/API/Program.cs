using TrackSense.API.AlertService.Services;
using TrackSense.API.AlertService.Repositories;
using TrackSense.API.AlertService.Models;

namespace TrackSense.API.AlertService;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();

        builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

        builder.Services.AddSingleton(new Dictionary<long, TimedMetricEvent>());
        builder.Services.AddSingleton(new Dictionary<int, Retry>());

        builder.Services.AddDbContext<AlertServiceContext>();
        builder.Services.AddCrudRepositories();
        builder.Services.AddCrudServices();

        builder.Services.AddScoped<ISender, TelegramSender>();
        builder.Services.AddScoped<INotificationService, NotificationService>();
        builder.Services.AddScoped<IMetricAnalyzerService, MetricAnalyzerService>();
        builder.Services.AddScoped<IRulesRepository, RulesRepository>();
        builder.Services.AddScoped<IEventsRepository, EventsRepository>();
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        builder.Services
            .AddControllers()
            .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
        );
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            options.RoutePrefix = string.Empty;
        });

        if (app.Environment.IsDevelopment())
        {
            using (AlertServiceContext db = new AlertServiceContext(builder.Configuration))
            {
                AlertRule alertRule1 = new AlertRule
                {
                    ID = 1456,
                    Type = AlertType.Information,
                    CriticalValue = 10.252F,
                    Operator = Operator.LessThanOrEqual,
                    ValueName = "Volt",
                    Imei = 19786135
                };

                NotificationGroup group1 = new NotificationGroup
                {
                    ID = 1144,
                    Name = "SisunTransportation"
                };

                TelegramChat telegram1 = new TelegramChat
                {
                    ID = 14645,
                    TelegramChatId = "-4244865051",
                    NotificationGroupId = group1.ID,
                    NotificationGroup = group1
                };

                TelegramChat telegram2 = new TelegramChat
                {
                    ID = 7432,
                    TelegramChatId = "-1412524536",
                    NotificationGroupId = group1.ID,
                    NotificationGroup = db.Groups.FirstOrDefault(g => g.ID == group1.ID)
                };

                TelegramChat telegram3 = new TelegramChat
                {
                    ID = 962,
                    TelegramChatId = "-2075214531",
                    NotificationGroupId = group1.ID,
                    NotificationGroup = db.Groups.FirstOrDefault(g => g.ID == group1.ID)
                };

                AlertRuleNotificationGroup groupAlert1 = new AlertRuleNotificationGroup
                {
                    ID = 1234,
                    AlertRuleId = alertRule1.ID,
                    AlertRule = alertRule1,
                    NotificationGroupId = group1.ID,
                    NotificationGroup = group1
                };

                db.Rules.AddIfNotExists(alertRule1, r => r.ID == alertRule1.ID);
                db.Groups.AddIfNotExists(group1, g => g.ID == group1.ID);
                db.Telegrams.AddIfNotExists(telegram1, t => t.ID == telegram1.ID);
                db.Telegrams.AddIfNotExists(telegram2, t => t.ID == telegram2.ID);
                db.Telegrams.AddIfNotExists(telegram3, t => t.ID == telegram3.ID);
                db.GroupAlerts.AddIfNotExists(groupAlert1, ga => ga.ID == groupAlert1.ID);
                db.SaveChanges();

                var rules = db.Rules.ToList();
                foreach (AlertRule r in rules)
                {
                    var operString = Operator.GetName(typeof(Operator), r.Operator);
                    var typeString = AlertType.GetName(typeof(AlertType), r.Type);
                    Console.WriteLine($"{typeString}: {r.ValueName} value is {operString} {r.CriticalValue}");
                }
            }
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();
        app.UseCors();

        app.MapControllers();

        app.Run();
    }
}