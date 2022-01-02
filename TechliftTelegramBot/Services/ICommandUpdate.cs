using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace TechliftTelegramBot.Services
{
    public interface ICommandUpdate
    {
        public Task GetPlayers(Update update);
    }
}
