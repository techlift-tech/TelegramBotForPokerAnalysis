using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechliftTelegramBot.Models;
using Telegram.Bot.Types.ReplyMarkups;

namespace TechliftTelegramBot.Services
{
    public interface IGenerateKeyboard
    {
        public InlineKeyboardButton[][] GetInlineKeyboard(List<Player> _player);
        public InlineKeyboardButton[][] GetInlineKeyboardButtons(Dictionary<string,string> LimitTypes, Guid Player);
    }
}
