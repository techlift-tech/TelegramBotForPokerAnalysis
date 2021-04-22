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


namespace TechliftTelegramBot.Services
{
    
    public class AgencyInfo
    {
        private readonly HttpClient client = new();
        private Agency agency=new();
        public AgencyInfo()
        {
            client.BaseAddress = new Uri("https://localhost:44390/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<Agency> GetAgency(int userid)
        {
            HttpResponseMessage response = await client.GetAsync($"/api/Agency/{userid}");
            if (response.IsSuccessStatusCode)
            {
                agency = await response.Content.ReadAsAsync<Agency>();
            }
            agency.AgencyName = "agency1";
            agency.id = userid;
            return agency;
        }
    }
}
