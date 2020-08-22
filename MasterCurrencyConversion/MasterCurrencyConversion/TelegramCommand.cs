using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace MasterCurrencyConversion
{
    public abstract class TelegramCommand
    {       
        public abstract Task Execute(Message message, ITelegramBotClient client);

        public abstract bool Contains(Message message);
    }
}
