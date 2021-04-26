using System;
using System.Collections.Generic;

using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using TechliftTelegramBot.Models;
using TechliftTelegramBot.Services;

namespace TechliftTelegramBot.Services
{
    
    public class PlayerInfoService:IPlayerInfoService
    {
        private readonly HttpClient _client;
        private List<Player> player = new();
        private readonly ApplicationConfiguration _config;
        public PlayerInfoService(IOptions<ApplicationConfiguration> config, IHttpClientFactory client)
        {
            _config = config.Value;
            _client = client.CreateClient();
            _client.BaseAddress = new Uri(_config.BaseAddress);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


        public async Task<List<Player>> GetPlayer(int agencyId)
        {
            HttpResponseMessage response = await _client.GetAsync($"/api/Player/{agencyId}");
            if (response.IsSuccessStatusCode)
            {
                player = await response.Content.ReadAsAsync<List<Player>>();
            }
            else
            {
                player.Add(new Player()
                {
                    Id =1,
                    Name = "Player_1"
                });
                player.Add(new Player()
                {
                    Id = 2,
                    Name = "Player_2"
                });
                player.Add(new Player()
                {
                    Id = 3,
                    Name = "Player_3"
                });
            }
            return player;
        }

       
    }
}
