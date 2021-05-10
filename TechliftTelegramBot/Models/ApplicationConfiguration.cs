using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;

namespace TechliftTelegramBot.Models
{
    public class ApplicationConfiguration
    {
        [JsonProperty("APIToken")]
        public string APIToken { get; set; }

        [JsonProperty("BaseAddress")]
        public string BaseAddress { get; set; }

    }

}
