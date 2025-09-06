using TrackSense.API.MetricGateway.Services;

namespace TrackSense.API.MetricGateway;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Logging.ClearProviders();
        builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
        builder.Logging.AddConsole();

        // Add services to the container.

        builder.Services.AddControllers();
        builder.Services.AddLogging();

        builder.Services.AddHttpClient(); // Пока нужно, для считывания данных с эмулятора датчика

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddSingleton<IKafkaSender, KafkaSender>();
        builder.Services.AddSingleton<ByteJsonConverter>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            options.RoutePrefix = string.Empty;
        });

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Logger.LogInformation("Application started and configured");

        app.Run();
    }
}
