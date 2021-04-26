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
        private readonly HttpClient _client;
        private readonly ApplicationConfiguration _config;
        private readonly ILogger _logger;
        private Agency agency=new();
        public AgencyInfoService(IOptions<ApplicationConfiguration> config, ILogger<AgencyInfoService> logger, IHttpClientFactory client)
        {
            _config = config.Value;
            _logger = logger;
            _client = client.CreateClient();
            _client.BaseAddress = new Uri(_config.BaseAddress);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<Agency> GetAgency(int telegramId)
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync($"/api/Agency/{telegramId}");
                if (response.IsSuccessStatusCode)
                {
                    agency = await response.Content.ReadAsAsync<Agency>();
                    if (agency.id == null)
                    {
                        throw new NullReferenceException("didn't get Agency data");
                    }
                }
                else
                {
                    throw new ArgumentException("could not find agency related to this telegramId");
                }
            }
            catch(NullReferenceException exception)
            {
                _logger.LogError(exception.Message);
            }
            catch(ArgumentException exception)
            {
                _logger.LogError(exception.Message);
            }
            return agency;
        }
    }
}
