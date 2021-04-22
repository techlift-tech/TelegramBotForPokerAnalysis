using Newtonsoft.Json;

namespace TechliftTelegramBot.Models
{
    public class Agency
    {
        [JsonProperty("Id")]
        public int id { get; set; }

        [JsonProperty("AgencyName")]
        public string AgencyName { get; set; }
    }
}
