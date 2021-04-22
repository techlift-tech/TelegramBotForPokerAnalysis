using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Microsoft.Extensions.Configuration;
using TechliftTelegramBot.Services;
using Microsoft.Extensions.Logging;
using TechliftTelegramBot.Models;


namespace TechliftTelegramBot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TelegramBot : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _config;
        private readonly AgencyInfo agency = new();
        private readonly PlayerInfo player = new();
        private readonly TelegramBotClient botClient;
        private readonly GenerateKeyboard generateKeyboard = new();

        public TelegramBot(ILogger<TelegramBot> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
            botClient = new(_config["APIToken"]);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Update update)
        { 
            User me = await botClient.GetMeAsync();

            _logger.LogInformation($"Hello, World! I am user {me.Username} and my name is {me.FirstName}.");
            
            if (update.Message != null)
            {
                if (update.Message.Text == "/todays_profit")
                {
                    Agency _agency1 = await agency.GetAgency(update.Message.From.Id);
                    Console.WriteLine(_agency1.id.ToString());
                    List<Player> _player = await player.GetPlayer(_agency1.id);
                    await botClient.SendTextMessageAsync(
                          chatId: update.Message.Chat.Id,
                          text: "Choose User to get todays_profit: ",
                        replyMarkup: new InlineKeyboardMarkup(generateKeyboard.GetInlineKeyboard(_player))
                     );
                }
            }
            return Ok("Message has been received.");
        }  
    }
}