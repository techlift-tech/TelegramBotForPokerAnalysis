using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Microsoft.Extensions.Configuration;
using TechliftTelegramBot.Services;
using Microsoft.Extensions.Logging;
using TechliftTelegramBot.Models;
using Microsoft.Extensions.Options;

namespace TechliftTelegramBot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TelegramBotWebhookController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ApplicationConfiguration _config;
        private readonly IAgencyInfoService _agency;
        private readonly IPlayerInfoService _player;
        private readonly ITelegramBotClient _botClient;
        private readonly IGenerateKeyboard _generateKeyboard;
        private readonly List<string> commands = new() { "/todays_profit", "/remaining_limit", "/set_limit", "/week_profit" };

        public TelegramBotWebhookController(ILogger<TelegramBotWebhookController> logger, IOptions<ApplicationConfiguration> config, IAgencyInfoService agency, IPlayerInfoService player, ITelegramBotClient botClient, IGenerateKeyboard generateKeyboard)
        {
            _logger = logger;
            _config = config.Value;
            _botClient = botClient;
            _agency = agency;
            _player = player;
            _generateKeyboard = generateKeyboard;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Update update)
        { 
            await _botClient.SendChatActionAsync(
                        chatId: update.Message.From.Id,
                        chatAction: ChatAction.Typing
                        );
             if (update.Message != null)
            {
                if (commands.Contains(update.Message.Text))
                {
                    List<Player> Players = await _player.GetPlayer(_agency.GetAgency(update.Message.From.Id).Id);
                    string text = string.Empty;
                    switch (update.Message.Text)
                    {
                        case "/todays_profit":
                            text = "today's profit of Players";
                            break;
                        case "/remaining_limit":
                            text = "remaining limit of Players";
                            break;
                        case "/set_limit":
                            text = "select to set limit of Players";
                            break;
                        case "/week_profit":
                            text = "select to set weekly limit of Players";
                            break;
                        default:
                            break;
                    }
                    await _botClient.SendTextMessageAsync(
                          chatId: update.Message.Chat.Id,
                          text: text,
                          replyMarkup: new InlineKeyboardMarkup(_generateKeyboard.GetInlineKeyboard(Players))
                     ) ;
                }
            }
            return Ok("Message has been received.");
        }  
    }
}