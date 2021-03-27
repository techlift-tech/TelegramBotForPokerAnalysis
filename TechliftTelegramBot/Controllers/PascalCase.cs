using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Newtonsoft.Json;
using System.Web;
using TechliftTelegramBot.Models;
using Microsoft.Extensions.Configuration;

namespace TechliftTelegramBot.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PascalCase : ControllerBase
    {
        IConfiguration _config;
        public PascalCase(IConfiguration config)
        {
            _config = config;
        }
       
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("hello world");
        }

        [HttpPost]
        public IActionResult Post(JObject payload)
        {
            var privatechat = payload.ToObject<NewMessage>();
            var botClient = new TelegramBotClient(_config["APIToken"]);
            var me = botClient.GetMeAsync().Result;
            Console.WriteLine(
              $"Hello, World! I am user {me.Username} and my name is {me.FirstName}.");
            if (privatechat.Message != null)
            {
                if (privatechat.Message.Text == "/hello")
                {
                    botClient.SendTextMessageAsync(
                        chatId: privatechat.Message.Chat.Id,
                        text: "hello " + privatechat.Message.From.FirstName);
                }
               
                else if (privatechat.Message.Text == "tell me time")
                {
                    botClient.SendTextMessageAsync(
                        chatId: privatechat.Message.Chat.Id,
                        text: "Current time is = " + DateTime.Now.ToString("h:mm:ss tt"));
                }
           
                else if (privatechat.Message.Text != null)
                {
                    if (privatechat.Message.Text.StartsWith("/"))
                    {
                        botClient.SendTextMessageAsync(
                            chatId: privatechat.Message.Chat.Id,
                            text: "hello " + privatechat.Message.From.FirstName + " you said \n" + privatechat.Message.Text);
                    }
                    return Ok(payload.ToString());
                }
            }
            return Ok(payload.ToString());
        }
    }
}
