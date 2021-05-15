using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechliftTelegramBot.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TechliftTelegramBot.Services
{
    public class CommandUpdate:ICommandUpdate
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ApplicationConfiguration _config;
        public IEnumerable<BotCommand> AllCommands;
        private readonly IAgencyInfoService _agency;
        private readonly IPlayerInfoService _player;
        private readonly IGenerateKeyboard _generateKeyboard;

        public CommandUpdate(ITelegramBotClient botClient, IOptions<ApplicationConfiguration> config, IAgencyInfoService agency, IPlayerInfoService player, IGenerateKeyboard generateKeyboard)
        {
            _botClient = botClient;
            _config = config.Value;
            AllCommands = config.Value.AllCommands;
            _agency = agency;
            _player = player;
            _generateKeyboard = generateKeyboard;
        }

        public async Task GetPlayers(Update update)
        {
            if (update.Message != null)
            {
                update.Message.Text = update.Message.Text.TrimStart('/');
                if (AllCommands.Any(x => x.Command.ToString() == update.Message.Text) )
                {
                    Agency agency = await _agency.GetAgency(update.Message.From.Id);
                    if (agency == null)
                    {
                        await _botClient.SendTextMessageAsync(
                          chatId: update.Message.Chat.Id,
                          text: "agency not found");
                        return;
                    }
                    List<Player> Players = await _player.GetPlayer(agency.Id);
                    BotCommand cmd = AllCommands.FirstOrDefault(x =>  update.Message.Text.StartsWith(x.Command.ToString()));
                    await _botClient.SendTextMessageAsync(
                          chatId: update.Message.From.Id,
                          text: cmd.Command.ToString()+" of Players",
                          replyMarkup: new InlineKeyboardMarkup(_generateKeyboard.GetInlineKeyboard(Players))
                     );
                    return;
                }
                else
                {
                    await _botClient.SendTextMessageAsync(
                        chatId: update.Message.From.Id,
                        text: "sorry, can't recognise command"
                        );
                    return;
                }
            }
            return;
        }
    }
}
