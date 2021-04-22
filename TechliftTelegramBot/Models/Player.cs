using Newtonsoft.Json;
namespace TechliftTelegramBot.Models
{
    public partial class Player
    {
        [JsonProperty("Id")]
        public int Id { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Agent")]
        public Agency Agent { get; set; }

        [JsonProperty("WeeklyLimit")]
        public long WeeklyLimit { get; set; }

        [JsonProperty("DailyLimit")]
        public long DailyLimit { get; set; }
    }
}
