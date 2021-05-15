using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TechliftTelegramBot.Models;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TechliftTelegramBot.Services
{
    public class CallbackQueryUpdate : ICallbackQueryUpdate
    {
        public IEnumerable<BotCommand> AllCommands;
        private readonly IAgencyInfoService _agency;
        private readonly IPlayerInfoService _player;
        private readonly IHttpClientFactory _client;
        private readonly ApplicationConfiguration _config;
        private readonly ITelegramBotClient _botClient;
        ProfitLoss profitLoss;

        public CallbackQueryUpdate(IOptions<ApplicationConfiguration> config, IAgencyInfoService agency,IPlayerInfoService player, IHttpClientFactory factory, ITelegramBotClient botClient)
        {
            AllCommands = config.Value.AllCommands;
            _agency = agency;
            _player = player;
            _client = factory;
            _config = config.Value;
            _botClient = botClient;
        }

        public async Task GetResults(Update update)
        {
            update.CallbackQuery.Message.Text = update.CallbackQuery.Message.Text.TrimStart('/');
            if (AllCommands.Any(x => update.CallbackQuery.Message.Text.StartsWith(x.Command.ToString())))
            {
                Agency agency = await _agency.GetAgency(update.CallbackQuery.From.Id);
                Player player = await _player.GetPlayer(agency.Id, Guid.Parse(update.CallbackQuery.Data));
                BotCommand command = AllCommands.FirstOrDefault(x => update.CallbackQuery.Message.Text.StartsWith(x.Command.ToString()));
                switch(command.Command.ToString()){
                    case "todays_profit":
                        goto todays_profit;
                    case "weekly_profit":
                        goto weekly_profit;
                    case "set_limit":
                        goto set_limit;
                    case "remaining_limit":
                        goto remaining_limit;
                    default:
                        break;
                }
            todays_profit:
                HttpClient client = _client.CreateClient("agency");
                client.BaseAddress = new Uri(_config.BaseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync($"/api​/ProfitLoss​/{agency.Id}​/{update.CallbackQuery.Data}");
                if (response.IsSuccessStatusCode)
                {
                    profitLoss = await response.Content.ReadAsAsync<ProfitLoss>();
                }
                else
                {
                    profitLoss = new()
                    {
                        TodaysProfitLoss = 1200,
                        WeeklyProfitLoss = 2400
                    };
                }
                await _botClient.SendTextMessageAsync(
                    chatId: update.CallbackQuery.From.Id,
                    text: "todays's profit of "+player.Name.ToString() + " is " + profitLoss.TodaysProfitLoss.ToString()
                    );
                client.Dispose();
                return;
            set_limit:
                return;
            weekly_profit:
                HttpClient client1 = _client.CreateClient("agency");
                client1.BaseAddress = new Uri(_config.BaseAddress);
                client1.DefaultRequestHeaders.Accept.Clear();
                client1.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response1 = await client1.GetAsync($"/api​/ProfitLoss​/{agency.Id}​/{update.CallbackQuery.Data}");
                if (response1.IsSuccessStatusCode)
                {
                    profitLoss = await response1.Content.ReadAsAsync<ProfitLoss>();
                }
                else
                {
                    profitLoss = new()
                    {
                        TodaysProfitLoss = 1200,
                        WeeklyProfitLoss = 2400
                    };
                }
                await _botClient.SendTextMessageAsync(
                    chatId: update.CallbackQuery.From.Id,
                    text: "weekly profit is " + profitLoss.WeeklyProfitLoss.ToString()
                    );
                client1.Dispose();
                return;
            remaining_limit:
                return;
            }
            return;
        }
    }
}
