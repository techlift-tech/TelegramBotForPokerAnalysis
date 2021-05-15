using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechliftTelegramBot.Controllers;
using TechliftTelegramBot.Models;
using TechliftTelegramBot.Services;
using Telegram.Bot.Types;

namespace TechliftTelegramBot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Application Started");
            IHost host = CreateHostBuilder(args).Build();

            using (IServiceScope serviceScope = host.Services.CreateScope())
            {
                IServiceProvider services = serviceScope.ServiceProvider;

                try
                {
                    IBotCommandCheckService myDependency = services.GetRequiredService<IBotCommandCheckService>();
                    System.Threading.Timer timer = new(
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

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
        }
    }
}
