using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechliftTelegramBot.Models;
using Telegram.Bot.Types.ReplyMarkups;

namespace TechliftTelegramBot.Services
{
    public class GenerateKeyboard
    {
        public InlineKeyboardButton[][] GetInlineKeyboard(List<Player> _player)
        {
            InlineKeyboardButton[] keyboardButtons = new InlineKeyboardButton[_player.Count + 1];
            int count = (int)Math.Ceiling(((decimal)_player.Count + 1) / 2);
            Console.WriteLine(count);
            InlineKeyboardButton[][] keyboardInline = new InlineKeyboardButton[count][];
            for (int i = 0; i < _player.Count; i++)
            {
                keyboardButtons[i] = new InlineKeyboardButton
                {
                    Text = _player[i].Name,
                    CallbackData = _player[i].Id.ToString(),
                };
            }
            keyboardButtons[_player.Count] = new InlineKeyboardButton
            {
                Text = "All Players",
                CallbackData = "All Players",
            };
            for (int i = 0; i < count; i++)
            {
                keyboardInline[i] = i == count - 1 && keyboardButtons.Length % 2 == 1
                    ? (new InlineKeyboardButton[] { keyboardButtons[_player.Count] })
                    : (new InlineKeyboardButton[] { keyboardButtons[i * 2], keyboardButtons[(i * 2) + 1] });
            }
            return keyboardInline;
        }
    }
}
