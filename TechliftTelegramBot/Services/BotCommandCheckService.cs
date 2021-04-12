using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TechliftTelegramBot.Services
{
    public class BotCommandCheckService : IBotCommandCheckService
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _config;

        public BotCommandCheckService(ILogger<BotCommandCheckService> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        private BotCommand[] CommandsToBeSet;
        public BotCommand[] GetCommandsToBeSetVariable()
        {
            return CommandsToBeSet;
        }
        public void SetCommandsToBeSetVariable(BotCommand[] value)
        {
            CommandsToBeSet = value;
        }

        public TelegramBotClient botClient { get; set; }

        private readonly IEnumerable<BotCommand> AllCommands = new BotCommand[]
            {
                new BotCommand{Command="todays_profit",Description="get today's profit"},
                new BotCommand{Command="remaining_limit",Description="get remaining limit"},
                new BotCommand{Command="set_limit",Description="set limit of a user"},
                new BotCommand{Command="week_profit",Description="get weekly profit"},
            };

        public void CheckCommands()
        {

            botClient = new TelegramBotClient(_config["APIToken"]);
            CommandsToBeSet= botClient.GetMyCommandsAsync().Result;

            for (int i = 0; i < AllCommands.Count(); ++i)
            {
                if (CommandsToBeSet.Any(a => a.Command == AllCommands.ElementAt(i).Command))
                {
                    Console.WriteLine("found the command: " + AllCommands.ElementAt(i).Command);
                }
                else
                {
                    Console.WriteLine("command not found, registreting BotCommands");
                    botClient.SetMyCommandsAsync(AllCommands);
                }

            }
        }
    }
}
