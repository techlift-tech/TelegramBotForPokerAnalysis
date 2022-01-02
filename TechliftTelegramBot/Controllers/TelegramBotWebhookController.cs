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
using System.Linq;

namespace TechliftTelegramBot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TelegramBotWebhookController : ControllerBase
    {
        private readonly IUpdatesHandler _updatesHandler;

        public TelegramBotWebhookController(IUpdatesHandler updatesHandler)
        {
            _updatesHandler = updatesHandler;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Update update)
        {
            return await _updatesHandler.GetUpdates(update) ? Ok("message received") : Ok("Unrecognized type or user");
        }  
    }
}