﻿using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace MasterCurrencyConversion
{
    public class StartCommand : TelegramCommand
    {
        public override bool Contains(Message message)
        {
            if (message.Type != MessageType.Text)
                return false;

            return message.Text.Contains("Start");
        }

        public async override Task Execute(Message message, ITelegramBotClient botClient)
        {
            var chatId = message.Chat.Id;

            var keyBoard = new ReplyKeyboardMarkup
            {
                Keyboard = new[]
               {
                   new[]
                    {
                        new KeyboardButton("Start")
                    },
                }
            };
            if (message.Text.Equals("Start", StringComparison.InvariantCultureIgnoreCase))
            {
                await botClient.SendTextMessageAsync(chatId, "Please, enter the date DD.MM.YYYY + currency code\nFor example: 14.07.2014 USD",
                     parseMode: ParseMode.Html, false, false, 0, keyBoard);
            }
        }
    }
}
