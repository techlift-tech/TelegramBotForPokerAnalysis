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
        private readonly TelegramBotClient _botclient;
        private readonly IEnumerable<BotCommand> AllCommands = new BotCommand[]
            {
                new BotCommand{Command="todays_profit",Description="get today's profit"},
                new BotCommand{Command="remaining_limit",Description="get remaining limit"},
                new BotCommand{Command="set_limit",Description="set limit of a user"},
                new BotCommand{Command="week_profit",Description="get weekly profit"},
            };
       
        public BotCommandCheckService(ILogger<BotCommandCheckService> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
            _botclient =new TelegramBotClient(_config["APIToken"]);
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

        public async void CheckCommands()
        {
            CommandsToBeSet = await _botclient.GetMyCommandsAsync();
            bool CommandsNeedToReset = false;
            foreach (BotCommand botCommand in AllCommands)
            {
                if (CommandsToBeSet.Any(any => any.Command == botCommand.Command) != true)
                {
                    CommandsNeedToReset = true;
                    _logger.LogInformation($"command {botCommand.Command} not found.");
                }
            }
            if(CommandsNeedToReset == true)
            {
                _logger.LogInformation("some commands were not found, resetting commands.");
                await _botclient.SetMyCommandsAsync(AllCommands);
            }
            else
            {
                _logger.LogInformation("All commands are registered already");
            }
        }
    }
}
