using System;
using System.Collections.Generic;

using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using TechliftTelegramBot.Models;
using TechliftTelegramBot.Services;

namespace TechliftTelegramBot.Services
{
    
    public class PlayerInfo
    {
        private readonly HttpClient client = new();
        private List<Player> player = new();

        public PlayerInfo()
        {
            client.BaseAddress = new Uri("https://localhost:44390/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public async Task<List<Player>> GetPlayer(int agencyId)
        {
            HttpResponseMessage response = await client.GetAsync($"/api/Player/{agencyId}");
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
