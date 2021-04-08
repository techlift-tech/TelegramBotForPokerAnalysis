using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TechliftTelegramBot.customClasses
{
    public class Commands : ICommands
    {
        IConfiguration _config;
        public Commands(IConfiguration config){
            _config = config;
        }
        public BotCommand[] commandsToBeSet { get ; set ; }
        
        public TelegramBotClient botClient { get; set; }

        public void getCommands()
        {
            botClient= new TelegramBotClient(_config["APIToken"]);
            commandsToBeSet = botClient.GetMyCommandsAsync().Result;
            for (int i = 0; i < commandsToBeSet.Count(); ++i)
            {
                Console.WriteLine(commandsToBeSet[i].Command+" "+commandsToBeSet[i].Description);
            }
        }
    }
}
