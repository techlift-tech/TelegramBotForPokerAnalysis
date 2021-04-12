using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechliftTelegramBot.Models;
using TechliftTelegramBot.Services;
namespace TechliftTelegramBot
{
    public class Program
    {
        
        public static void Main(string[] args)
        {
            Console.WriteLine("Application Started");
            var host = CreateHostBuilder(args).Build();

            using (var serviceScope = host.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                try
                {
                    var myDependency = services.GetRequiredService<IBotCommandCheckService>();
                    var timer = new System.Threading.Timer(
                        e => myDependency.CheckCommands(),
                        null,
                        TimeSpan.Zero,
                        TimeSpan.FromHours(1));
                }
                catch (Exception exception)
                {
                    ILogger<Program> logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(LogEvents.BotCommandCheckServiceTimerError, exception, @"Error in starting the BotCommandCheckService");
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
