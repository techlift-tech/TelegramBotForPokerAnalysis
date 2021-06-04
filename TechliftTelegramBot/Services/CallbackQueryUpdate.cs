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
using Telegram.Bot.Types.ReplyMarkups;

namespace TechliftTelegramBot.Services
{
    public class CallbackQueryUpdate : ICallbackQueryUpdate
    {
        public IEnumerable<BotCommand> AllCommands;
        private readonly Dictionary<string, string> LimitTypes;
        private readonly IAgencyInfoService _agency;
        private readonly IPlayerInfoService _player;
        private readonly IHttpClientFactory _client;
        private readonly ApplicationConfiguration _config;
        private readonly ITelegramBotClient _botClient;
        private List<ProfitLoss> profitLoss = new();
        private readonly IGenerateKeyboard _generateKeyboard;

        public CallbackQueryUpdate(IOptions<ApplicationConfiguration> config, IAgencyInfoService agency,IPlayerInfoService player, IHttpClientFactory factory, ITelegramBotClient botClient, IGenerateKeyboard generateKeyboard)
        {
            AllCommands = config.Value.AllCommands;
            LimitTypes = config.Value.LimitTypes;
            _agency = agency;
            _player = player;
            _client = factory;
            _config = config.Value;
            _botClient = botClient;
            _generateKeyboard = generateKeyboard;
           
        }

        public async Task GetResults(Update update)
        {
            update.CallbackQuery.Message.Text = update.CallbackQuery.Message.Text.TrimStart('/');
            if (AllCommands.Any(x => update.CallbackQuery.Message.Text.StartsWith(x.Command.ToString())))
            {
                Agency agency = await _agency.GetAgency(update.CallbackQuery.From.Id);
                if (agency == null)
                {
                    await _botClient.SendTextMessageAsync(
                      chatId: update.CallbackQuery.From.Id,
                      text: "agency not found");
                    return;
                }
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
                        await _botClient.SendTextMessageAsync(
                     chatId: update.CallbackQuery.From.Id,
                     text: "call back not recognized");
                        break;
                }
            todays_profit:
                if (update.CallbackQuery.Data == "All Players")
                {
                    HttpClient client = _client.CreateClient("agency");
                    client.BaseAddress = new Uri(_config.BaseAddress);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = await client.GetAsync($"/mocks/telegrambot/bot:main/12898554/api/ProfitLoss/​{agency.Id}");
                    if (response.IsSuccessStatusCode)
                    {
                        profitLoss = await response.Content.ReadAsAsync<List<ProfitLoss>>();
                        Console.WriteLine(profitLoss.Count.ToString());
                    }
                    else
                    {
                        profitLoss.Add(new() {
                            TodaysProfitLoss = 2300,
                            WeeklyProfitLoss = 4600
                        });
                        profitLoss.Add(new()
                        {
                            TodaysProfitLoss = 2300,
                            WeeklyProfitLoss = 4600
                        });
                    }
                    double profit = 0;
                    foreach(ProfitLoss profitLoss in profitLoss)
                    {
                        profit += profitLoss.TodaysProfitLoss;
                    }
                    await _botClient.EditMessageTextAsync(
                            chatId: update.CallbackQuery.From.Id,
                            messageId: update.CallbackQuery.Message.MessageId,
                            text: "todays's profit of All Players is " + profit.ToString()
                            );
                    client.Dispose();
                    return;
                }
                else
                {
                    List<Player> player = await _player.GetPlayer(agency.Id, Guid.Parse(update.CallbackQuery.Data));
                    if (player[0].Id == Guid.Empty)
                    {
                        await _botClient.AnswerCallbackQueryAsync(
                            callbackQueryId: update.CallbackQuery.Id,
                            text: "agency dont have any players"
                            );
                        return;
                    }
                    HttpClient client = _client.CreateClient("agency");
                    client.BaseAddress = new Uri(_config.BaseAddress);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = await client.GetAsync($"/mocks/telegrambot/bot:main/12898554/api/ProfitLoss/​{agency.Id}/{update.CallbackQuery.Data}");
                    if (response.IsSuccessStatusCode)
                    {
                        ProfitLoss profit = await response.Content.ReadAsAsync<ProfitLoss>();
                        profitLoss.Add(profit);
                    }
                    else
                    {
                        profitLoss.Add(new()
                        {
                            TodaysProfitLoss = 1200,
                            WeeklyProfitLoss = 2400
                        });
                    }

                    await _botClient.EditMessageTextAsync(
                            chatId: update.CallbackQuery.From.Id,
                            messageId: update.CallbackQuery.Message.MessageId,
                            text: "todays's profit of " + player[0].Name.ToString() + " is " + profitLoss[0].TodaysProfitLoss.ToString()
                            );
                    client.Dispose();
                    return;
                }
            set_limit:
                await _botClient.SendTextMessageAsync(
                    chatId: update.CallbackQuery.From.Id,
                    text: "set limit",
                    replyMarkup: new InlineKeyboardMarkup(_generateKeyboard.GetInlineKeyboardButtons(LimitTypes,Guid.Parse(update.CallbackQuery.Data))));
                await _botClient.EditMessageReplyMarkupAsync(
                        chatId: update.CallbackQuery.From.Id,
                        messageId: update.CallbackQuery.Message.MessageId
                        );
                return;
            weekly_profit:
                if (update.CallbackQuery.Data == "All Players")
                {
                    HttpClient client = _client.CreateClient("agency");
                    client.BaseAddress = new Uri(_config.BaseAddress);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = await client.GetAsync($"/mocks/telegrambot/bot:main/12898554/api/ProfitLoss/​{agency.Id}");
                    if (response.IsSuccessStatusCode)
                    {
                        profitLoss = await response.Content.ReadAsAsync<List<ProfitLoss>>();
                        Console.WriteLine(profitLoss.Count.ToString());
                    }
                    else
                    {
                        profitLoss.Add(new()
                        {
                            TodaysProfitLoss = 2300,
                            WeeklyProfitLoss = 4600
                        });
                        profitLoss.Add(new()
                        {
                            TodaysProfitLoss = 2300,
                            WeeklyProfitLoss = 4600
                        });
                    }
                    double profit = 0;
                    foreach (ProfitLoss profitLoss in profitLoss)
                    {
                        profit += profitLoss.WeeklyProfitLoss;
                    }
                    await _botClient.EditMessageTextAsync(
                            chatId: update.CallbackQuery.From.Id,
                            messageId: update.CallbackQuery.Message.MessageId,
                            text: "todays's profit of All Players is " + profit.ToString()
                            );
                    client.Dispose();
                    return;
                }
                else
                {
                    List<Player> player = await _player.GetPlayer(agency.Id, Guid.Parse(update.CallbackQuery.Data));
                    if (player[0].Id == Guid.Empty)
                    {
                        await _botClient.AnswerCallbackQueryAsync(
                            callbackQueryId: update.CallbackQuery.Id,
                            text: "agency dont have any players"
                            );
                        return;
                    }
                    HttpClient client = _client.CreateClient("agency");
                    client.BaseAddress = new Uri(_config.BaseAddress);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = await client.GetAsync($"/mocks/telegrambot/bot:main/12898554/api/ProfitLoss/​{agency.Id}/{update.CallbackQuery.Data}");
                    if (response.IsSuccessStatusCode)
                    {
                        ProfitLoss profit = await response.Content.ReadAsAsync<ProfitLoss>();
                        profitLoss.Add(profit);
                    }
                    else
                    {
                        profitLoss.Add(new()
                        {
                            TodaysProfitLoss = 1200,
                            WeeklyProfitLoss = 2400
                        });
                    }

                    await _botClient.EditMessageTextAsync(
                            chatId: update.CallbackQuery.From.Id,
                            messageId: update.CallbackQuery.Message.MessageId,
                            text: "weekly profit of " + player[0].Name.ToString() + " is " + profitLoss[0].WeeklyProfitLoss.ToString()
                            );
                    client.Dispose();
                    return;
                }
            remaining_limit:
                await _botClient.SendTextMessageAsync(
                    chatId: update.CallbackQuery.From.Id,
                    text: "select limit type",
                    replyMarkup: new InlineKeyboardMarkup(_generateKeyboard.GetInlineKeyboardButtons(LimitTypes, Guid.Parse(update.CallbackQuery.Data))));
                await _botClient.EditMessageReplyMarkupAsync(
                        chatId: update.CallbackQuery.From.Id,
                        messageId: update.CallbackQuery.Message.MessageId
                        );
                return;
            }
            else if (LimitTypes.Any(x => update.CallbackQuery.Data.StartsWith(x.Value.ToString())))
            {
                Agency agency = await _agency.GetAgency(update.CallbackQuery.From.Id);
                if (agency == null)
                {
                    await _botClient.SendTextMessageAsync(
                      chatId: update.CallbackQuery.From.Id,
                      text: "agency not found");
                    return;
                }
                List<Player> player = await _player.GetPlayer(agency.Id, Guid.Parse(update.CallbackQuery.Data));
                if (player[0].Id == Guid.Empty)
                {
                    await _botClient.AnswerCallbackQueryAsync(
                        callbackQueryId: update.CallbackQuery.Id,
                        text: "agency dont have any players"
                        );
                    return;
                }
            }
            return;
        }
    }
}
