using System.Diagnostics;
using ZipperAPI.Models;
using static System.Net.Mime.MediaTypeNames;

namespace ZipperAPI.Services;

public class ProcessService : IProcessHandler
{
    private readonly Dictionary<int, ProcessInfo> _processes = new();
    private int _counter = 0;

    private readonly ILogger<ProcessService> _logger;
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _env;
    private readonly ICacher _cacher;

    public ProcessService(
        ILogger<ProcessService> logger,
        IConfiguration configuration,
        IWebHostEnvironment env,
        ICacher cacher)
    {
        _logger = logger;
        _configuration = configuration;
        _env = env;
        _cacher = cacher;
    }

    public StatusType ProcessStatus(int processId)
    {
        if (_processes.TryGetValue(processId, out var processInfo))
        {
            return processInfo.Status;
        }

        throw new KeyNotFoundException($"Process with Id {processId} does not exist.");
    }

    public FileStream ProcessStream(int processId)
    {
        if (!_processes.TryGetValue(processId, out var processInfo))
        {
            throw new KeyNotFoundException($"Process with Id {processId} was not found");
        }
        if(processInfo.Status != StatusType.Completed)
        {
            throw new KeyNotFoundException($"Process with Id {processId} has not finished");
        }
        FileStream reader = new FileStream(processInfo.DownloadPath, FileMode.Open);
        return reader;
    }

    public struct FileCacheInfo
    {
        public string[] Files;
        public ProcessInfo Info;

        public FileCacheInfo(string[] files, ProcessInfo info)
        {
            Files = files;
            Info = info;
        }
    }

    public int StartArchiveProcess(string[] files)
    {
        string check_cache = string.Join("", files);
        if (_cacher.TryGetCached(check_cache, out ProcessInfo? info))
        {
            _logger.LogInformation($"Archive for {files} was cached");
            return info.Id;
        }

        var archivePath = _configuration["ArchivePath"]
                          ?? throw new ArgumentException("Archive path not set");

        var folderPath = _configuration["FolderPath"]
                         ?? throw new ArgumentException("Folder path not set");

        var archiverPath = Environment.GetEnvironmentVariable("ARCHIVER_PATH")
                           ?? throw new InvalidOperationException("ArchiverPath not configured.");

        int id = _counter++;
        var workingDir = Path.Combine(_env.ContentRootPath, folderPath);
        var outputPath = Path.Combine(_env.ContentRootPath, archivePath, $"archive_{id}.zip");

        var args = BuildArchiverArguments(outputPath, files);

        var process = CreateProcess(archiverPath, args, workingDir);

        if (!process.Start())
            throw new InvalidOperationException("Failed to start archiver process.");

        ProcessInfo processInfo = new ProcessInfo
        {
            Id = id,
            Process = process,
            Status = StatusType.InProgress,
            DownloadPath = outputPath
        };
        _processes[id] = processInfo;

        _logger.LogInformation("Started archiving process {ProcessId} -> {ArchivePath}", id, outputPath);

        MonitorProcessAsync(id, process, new FileCacheInfo(files, processInfo));

        return id;
    }

    private static string BuildArchiverArguments(string outputPath, string[] files) =>
        $"a -tzip \"{outputPath}\" {string.Join(" ", files.Select(f => $"\"{f}\""))}";

    private static Process CreateProcess(string fileName, string arguments, string workingDir) =>
        new()
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = workingDir
            },
            EnableRaisingEvents = true
        };

    private void MonitorProcessAsync(int id, Process process, FileCacheInfo info) =>
        Task.Run(async () =>
        {
            await process.WaitForExitAsync();

            if (!_processes.TryGetValue(id, out var processInfo))
                return;

            if (process.ExitCode == 0)
            {
                processInfo.Status = StatusType.Completed;
                _logger.LogInformation("Process {ProcessId} completed successfully.", id);
                string cache = string.Join("", info.Files);
                _cacher.Cache(cache, info.Info);
            }
            else
            {
                processInfo.Status = StatusType.Failed;
                var error = await process.StandardError.ReadToEndAsync();
                _logger.LogError("Process {ProcessId} failed with exit code {ExitCode}. Error: {Error}",
                    id, process.ExitCode, error);
            }
        });
}
