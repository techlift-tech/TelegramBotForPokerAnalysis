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


namespace TechliftTelegramBot.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class TelegramBot : ControllerBase
    {
        IConfiguration _config;
        public TelegramBot(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public IActionResult Get()
        {

            return Ok("hello world");
        }

        [HttpPost]
        public IActionResult Post(Update update)
        {
            var botClient = new TelegramBotClient(_config["APIToken"]);
            var me = botClient.GetMeAsync().Result;

            Console.WriteLine(
              $"Hello, World! I am user {me.Username} and my name is {me.FirstName}.");

            if (update.Message != null)
            {
                if (update.Message.Text == "/hello")
                {
                    botClient.SendTextMessageAsync(
                        chatId: update.Message.Chat.Id,
                        text: "hello " + update.Message.From.FirstName);
                }
            }
            return Ok("Message has been received.");
        }
    }
}
