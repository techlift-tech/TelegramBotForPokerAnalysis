using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechliftTelegramBot.Models
{
    public partial class ProfitLoss
    {
        [JsonProperty("Player")]
        public Player Player { get; set; }

        [JsonProperty("WeeklyProfitLoss")]
        public double WeeklyProfitLoss { get; set; }

        [JsonProperty("TodaysProfitLoss")]
        public double TodaysProfitLoss { get; set; }

        [JsonProperty("TodaysRemainingLimit")]
        public double TodaysRemainingLimit { get; set; }

        [JsonProperty("WeeksRamainingLimit")]
        public double WeeksRamainingLimit { get; set; }
    }
}
