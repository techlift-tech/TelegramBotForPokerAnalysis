using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TechliftTelegramBot.Models
{
    public class ApplicationConfiguration
    {
        [JsonProperty("APIToken")]
        public string APIToken { get; set; }

        [JsonProperty("BaseAddress")]
        public string BaseAddress { get; set; }

        public IEnumerable<BotCommand> AllCommands = new BotCommand[]
            {
                new BotCommand{Command="todays_profit",Description="get today's profit"},
                new BotCommand{Command="remaining_limit",Description="get remaining limit"},
                new BotCommand{Command="set_limit",Description="set limit of a user"},
                new BotCommand{Command="weekly_profit",Description="get weekly profit"},
            };
        public Dictionary<string,string> LimitTypes = new()
        {
            { "Today's Remaining Limit", "TodaysRemainingLimit" },
            { "Week's Remaining Limit", "WeeksRamainingLimit" }
        };
            
}

}
