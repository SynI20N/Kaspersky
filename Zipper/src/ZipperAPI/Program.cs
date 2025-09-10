using ZipperAPI.Services;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddSingleton<IConfiguration>(configuration);
builder.Services.AddSingleton<IProcessHandler, ProcessService>();
builder.Services.AddSingleton<IFolderService, FolderService>();
builder.Services.AddSingleton<ICacher, CacherService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

var archivePath = configuration["ArchivePath"]
                  ?? throw new ArgumentException("Archive path not set");
var outputPath = Path.Combine(builder.Environment.ContentRootPath, archivePath);

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


app.Run();