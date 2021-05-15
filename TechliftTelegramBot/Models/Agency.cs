using Newtonsoft.Json;
using System;

namespace TechliftTelegramBot.Models
{
    public class Agency
    {
        [JsonProperty("Id")]
        public Guid Id { get; set; }

        [JsonProperty("AgencyName")]
        public string AgencyName { get; set; }
    }
}
