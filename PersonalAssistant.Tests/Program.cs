﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using PersonalAssistant.Core;
using PersonalAssistant.Tests;

internal class Program
{
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
                var modelPath = hostContext.Configuration.GetSection("ModelPath");
                var apiKey = hostContext.Configuration.GetSection("ApiKey");

                services.CoreConfigure(modelPath.Value, apiKey.Value);

                services.AddHostedService<StartupConsole>();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Не удалось зарегистрировать зависимости при запуске \n {ex}");
            }
        });
}