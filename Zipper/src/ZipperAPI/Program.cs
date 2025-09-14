using ZipperAPI.Services;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddSingleton<IConfiguration>(configuration);
builder.Services.AddSingleton<IFolderService, FolderService>();
builder.Services.AddScoped<IProcessHandler, ProcessService>();
builder.Services.AddScoped<ICacher, CacherService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var folderService = builder.Services.BuildServiceProvider().GetService<IFolderService>();
var outputPath = folderService.GetArchivesPath();

CleanWorkingDir(outputPath);

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

static void CleanWorkingDir(string outputPath)
{
    if (!Directory.Exists(outputPath))
    {
        Directory.CreateDirectory(outputPath);
    }
    else
    {
        foreach (var file in Directory.GetFiles(outputPath))
        {
            File.Delete(file);
        }

        foreach (var dir in Directory.GetDirectories(outputPath))
        {
            Directory.Delete(dir, recursive: true);
        }
    }
}