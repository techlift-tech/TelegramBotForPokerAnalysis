using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace TechliftTelegramBot.Services
{
    public interface IUpdatesHandler
    {
        public Task<bool> GetUpdates(Update update);
    }
}
