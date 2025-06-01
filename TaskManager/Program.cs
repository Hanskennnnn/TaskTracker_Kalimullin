using Library.Interfaces;
using Library.Repositories;
using Library.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Library.Data;
using NLog;
using NLog.Extensions.Logging;
using NLog.Targets;
using NLog.Config;

namespace TaskManager;

internal static class Program
{
    [STAThread]
    static async Task Main()
    {

        var config = new LoggingConfiguration();


        var logfile = new FileTarget("logfile")
        {
            FileName = "logs/taskmanager.log",
            Layout = "${longdate} | ${level:uppercase=true} | ${message} ${exception:format=tostring}",
            ConcurrentWrites = true,
            KeepFileOpen = false
        };
        config.AddTarget(logfile);

        config.AddRule(NLog.LogLevel.Info, NLog.LogLevel.Fatal, logfile);


        LogManager.Configuration = config;

        var host = Host.CreateDefaultBuilder()
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
                logging.AddNLog();
            })
            .ConfigureServices(services =>
            {
                services.AddDbContext<AppDbContext>();

                services.AddScoped<ITaskRepository, TaskRepository>();
                services.AddScoped<ITaskService, TaskService>();

                services.AddScoped<Form1>();
            })
            .Build();

        using (var scope = host.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await db.Database.EnsureCreatedAsync();
        }

        ApplicationConfiguration.Initialize();
        var form = host.Services.GetRequiredService<Form1>();
        Application.Run(form);

        LogManager.Shutdown(); 
    }
}
