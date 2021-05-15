using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechliftTelegramBot.Models;
using TechliftTelegramBot.Services;
using Telegram.Bot;

namespace TechliftTelegramBot
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IBotCommandCheckService, BotCommandCheckService>();
            services.AddScoped<IAgencyInfoService, AgencyInfoService>();
            services.AddScoped<IPlayerInfoService, PlayerInfoService>();
            services.AddScoped<IGenerateKeyboard, GenerateKeyboard>();
            services.AddScoped<IUpdatesHandler, UpdatesHandler>();
            services.AddScoped<ICommandUpdate, CommandUpdate>();
            services.AddScoped<ICallbackQueryUpdate, CallbackQueryUpdate>();
            services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(_config.GetValue<string>("ApplicationConfiguration:APIToken")));
            services.Configure<ApplicationConfiguration>(this._config.GetSection("ApplicationConfiguration"));
            services.AddControllers().AddNewtonsoftJson();
            services.AddHttpClient();
            services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "TechliftTelegramBot", Version = "v1" }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TechliftTelegramBot v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
