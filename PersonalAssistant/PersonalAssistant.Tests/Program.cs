using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using PersonalAssistant.Core;
using PersonalAssistant.Core.Commands;
using PersonalAssistant.Core.Enums;
using PersonalAssistant.Tests;

internal static class Program
{
    private static readonly AppSettings AppSettings = new();
    
    private static void Main(string[] args)
    {
        try
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Local");

            CreateHostBuilder(args).Build().Run();
            while (Console.ReadKey().Key != ConsoleKey.Q)
            {
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            while (Console.ReadKey().Key != ConsoleKey.Q)
            {
            }
        }
    }

    private static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args)
        .UseEnvironment(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"))
        .ConfigureAppConfiguration((hostingContext, config) =>
        {
            config.SetBasePath(Directory.GetCurrentDirectory());
            config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            config.AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json",
                optional: true, reloadOnChange: true);
            config.AddCommandLine(args);
        })
        .ConfigureServices((hostContext, services) =>
        {
            try
            {
                hostContext.Configuration.Bind(AppSettings);
                
                services.ConfigureAssistant(options =>
                {
                    options.CommandHandleType = CommandHandleType.UseAll;
                    options.ThresholdRecognizeCommandPercent = 75;
                    options.LanguageModelPath = AppSettings.LanguageModelPath;
                    options.OpenAiKey = AppSettings.OpenAiKey;
                    options.WakeUpCommand = AppSettings.WakeUpCommand;
                });
               
                services.AddSingleton(AppSettings);

                services.AddHostedService<StartupConsole>();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Не удалось зарегистрировать зависимости при запуске \n {ex}");
            }
        });
}