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
        public BotCommand[] commandsToBeSet { get; set; }
        public TelegramBotClient botClient { get; set; }
        public void CheckCommands();
    }
}
