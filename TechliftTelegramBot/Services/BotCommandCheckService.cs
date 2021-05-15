using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechliftTelegramBot.Models;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TechliftTelegramBot.Services
{
    public class BotCommandCheckService : IBotCommandCheckService
    {
        private readonly ILogger _logger;
        private readonly ApplicationConfiguration _config;
        private readonly TelegramBotClient _botclient;
        private IEnumerable<BotCommand> CommandsToBeSet;
        public IEnumerable<BotCommand> AllCommands;
       
        public BotCommandCheckService(ILogger<BotCommandCheckService> logger, IOptions<ApplicationConfiguration> config)
        {
            _logger = logger;
            _config = config.Value;
            _botclient =new TelegramBotClient(_config.APIToken);
            AllCommands = config.Value.AllCommands;
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
