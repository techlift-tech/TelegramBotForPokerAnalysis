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
            // Console.WriteLine(update.Message.Chat.Type);
            var botClient = new TelegramBotClient(_config["APIToken"]);
            var me = botClient.GetMeAsync().Result;
            Console.WriteLine(
              $"Hello, World! I am user {me.Username} and my name is {me.FirstName}.");
            var cmd = botClient.GetMyCommandsAsync().Result;
            
            Console.WriteLine(cmd.Length);
            var rkm = new ReplyKeyboardMarkup();
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
                else if (update.Message.Text == "/getprofit")
                {
                    botClient.SendTextMessageAsync(
                        chatId: update.Message.Chat.Id,
                        text: "select user",
                        replyMarkup: new InlineKeyboardMarkup(new[]
                        {
                                new []
                                {
                                     InlineKeyboardButton.WithCallbackData ("user1", "/myCommand1"),
                                },
                                new []
                                {
                                    InlineKeyboardButton.WithCallbackData ("user2", "/myCommand2"),
                                },
                                new []
                                {
                                    InlineKeyboardButton.WithCallbackData ("user2", "/myCommand3"),
                                    InlineKeyboardButton.WithCallbackData("user3","/mycommand4")
                                }
                        })
                    );
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
            if (update.CallbackQuery != null)
            {
                Console.WriteLine(update.CallbackQuery.From.FirstName + " " + update.CallbackQuery.Data);
                botClient.SendTextMessageAsync(
                            chatId: update.CallbackQuery.Message.Chat.Id,
                            text: "hello " + update.CallbackQuery.From.FirstName + " you said \n" + update.CallbackQuery.Data);
            }
            return Ok(update.ToString());
        }
    } 
}
