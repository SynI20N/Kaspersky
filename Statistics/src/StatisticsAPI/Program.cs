using StatisticsAPI.Services;
using StatisticsDatabase.Context;
using StatisticsDatabase.Repositories;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddSingleton<IConfiguration>(configuration);
builder.Services.AddScoped<IFilePersistence, FilePersistenceDbService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<StatisticsDatabaseContext>();
ServiceCollectionExtensions.AddCrudRepositories(builder.Services);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();
app.UseDeveloperExceptionPage();

app.Run();