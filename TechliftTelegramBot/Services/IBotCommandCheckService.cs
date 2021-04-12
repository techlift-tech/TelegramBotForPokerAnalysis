using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TechliftTelegramBot.Services
{
    public interface IBotCommandCheckService
    {
        BotCommand[] GetCommandsToBeSetVariable();
        void SetCommandsToBeSetVariable(BotCommand[] value);
        public void CheckCommands();
    }
}
