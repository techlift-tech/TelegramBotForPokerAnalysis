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
using Telegram.Bot.Args;
using Newtonsoft.Json;
using System.Web;
using Microsoft.Extensions.Configuration;

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
        public IActionResult Post(Telegram.Bot.Types.Update update)
        {
            Console.WriteLine(update.Message.Chat.Type);
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
                else if (update.Message.Text == "tell me time")
                {
                    botClient.SendTextMessageAsync(
                        chatId: update.Message.Chat.Id,
                        text: "Current time is = " + DateTime.Now.ToString("h:mm:ss tt"));
                }
                else if (update.Message.Text != null)
                {
                    if (update.Message.Text.StartsWith("/"))
                    {
                        botClient.SendTextMessageAsync(
                            chatId: update.Message.Chat.Id,
                            text: "hello " + update.Message.From.FirstName + " you said \n" + update.Message.Text);
                    }
                    return Ok(update.ToString());
                }
            }
            return Ok(update.ToString());
        }
    }
}
