using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.Passport;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Args;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using Newtonsoft.Json;
using System.Web;
using Microsoft.Extensions.Configuration;
using TechliftTelegramBot.Services;
using Microsoft.Extensions.Logging;

namespace TechliftTelegramBot.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class TelegramBot : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _config;

        public TelegramBot(ILogger<TelegramBot> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        

        [HttpPost]
        public async Task<IActionResult> Post(Update update)
        {
            TelegramBotClient botClient = new(_config["APIToken"]);
           
            User me = await botClient.GetMeAsync();
            Console.WriteLine(
              $"Hello, World! I am user {me.Username} and my name is {me.FirstName}.");

            if (update.Message != null)
            {
                if (update.Message.Text == "/hello")
                {
                   await botClient.SendTextMessageAsync(
                         chatId: update.Message.Chat.Id, text: "hello " + update.Message.From.FirstName) ;
                }
            }
            return Ok("Message has been received.");
        }
    }
}
