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
        private readonly IHttpClientFactory _client;
        private List<Player> player = new();
        private readonly ApplicationConfiguration _config;
        public PlayerInfoService(IOptions<ApplicationConfiguration> config, IHttpClientFactory factory)
        {
            _config = config.Value;
            _client = factory;
        }

        public async Task<List<Player>> GetPlayer(Guid agencyId)
        {
            HttpClient client = _client.CreateClient("Player"); ;
            client.BaseAddress = new Uri(_config.BaseAddress);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = await client.GetAsync($"/mocks/telegrambot/bot:main/12898554/api/Player/{agencyId}");
            if (response.IsSuccessStatusCode)
            {
                player = await response.Content.ReadAsAsync<List<Player>>();
            }
            else
            {
                player.Add(new Player()
                {
                    Id = Guid.NewGuid(),
                    Name = "Player_1"
                });
                player.Add(new Player()
                {
                    Id = Guid.NewGuid(),
                    Name = "Player_2"
                });
                player.Add(new Player()
                {
                    Id = Guid.NewGuid(),
                    Name = "Player_3"
                });
            }
            return player;
        }
        public async Task<List<Player>> GetPlayer(Guid agencyId, Guid playerId)
        {
            HttpClient client = _client.CreateClient("Player"); ;
            client.BaseAddress = new Uri(_config.BaseAddress);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = await client.GetAsync($"/mocks/telegrambot/bot:main/12898554/api/Player/{agencyId}/{playerId}");
            if (response.IsSuccessStatusCode)
            {
                player = await response.Content.ReadAsAsync<List<Player>>();
                return player;
            }
            else
            {
                player.Add( new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Player_1"
                });
                return player;
            }
        }
    }
}
