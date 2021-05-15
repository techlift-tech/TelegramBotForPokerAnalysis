using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace TechliftTelegramBot.Services
{
    public interface ICallbackQueryUpdate
    {
        public Task GetResults(Update update);    
    }
}
