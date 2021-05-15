using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechliftTelegramBot.Models;
using TechliftTelegramBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace TechliftTelegramBot
{
    public class UpdatesHandler : IUpdatesHandler
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ICommandUpdate _commandUpdate;
        private readonly ICallbackQueryUpdate _callbackQueryUpdate;

        public UpdatesHandler(ITelegramBotClient botClient, ICommandUpdate  commandUpdate,ICallbackQueryUpdate callbackQueryUpdate)
        {
            _botClient = botClient;
            _commandUpdate = commandUpdate;
            _callbackQueryUpdate = callbackQueryUpdate;
        }
        public async Task<bool> GetUpdates(Update update)
        { 
            if (update.Type.ToString() == "CallbackQuery")
            {
                await _botClient.SendChatActionAsync(
                        chatId: update.CallbackQuery.From.Id,
                        chatAction: ChatAction.Typing
                        );
                await _callbackQueryUpdate.GetResults(update);
                return true;
            }
            else if (update.Type.ToString() == "Message")
            {
                await _botClient.SendChatActionAsync(
                        chatId: update.Message.Chat.Id,
                        chatAction: ChatAction.Typing
                        );
                await _commandUpdate.GetPlayers(update);
                return true;
            }
            else 
            {
                return false;
            }
        }
    }
}
