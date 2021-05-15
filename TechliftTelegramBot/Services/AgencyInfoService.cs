using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using Telegram.Bot.Types;
using TechliftTelegramBot.Models;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace TechliftTelegramBot.Services
{
    public class AgencyInfoService:IAgencyInfoService
    {
        private readonly IHttpClientFactory _client;
        private readonly ApplicationConfiguration _config;
        private readonly ILogger _logger;
        private Agency agency;
        public AgencyInfoService(IOptions<ApplicationConfiguration> config, ILogger<AgencyInfoService> logger, IHttpClientFactory factory)
        {
            _config = config.Value;
            _logger = logger;
            _client = factory;
        }

        public async Task<Agency> GetAgency(int telegramId)
        {
            HttpClient client = _client.CreateClient("agency");
            client.BaseAddress = new Uri(_config.BaseAddress);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = await client.GetAsync($"/api/Agency/{telegramId}");
            if (response.IsSuccessStatusCode)
            {
                agency = await response.Content.ReadAsAsync<Agency>();
                _logger.LogInformation("found agency related to telegram id");
            }
            else
            {
                agency = new()
                {
                    Id = Guid.NewGuid(),
                    AgencyName = "disha"
                };
            }
            client.Dispose();
            return agency;
        }
    }
}
