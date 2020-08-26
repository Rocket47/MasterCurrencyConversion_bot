using System;
using Telegram.Bot;
using Telegram.Bot.Args;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using System.Globalization;

namespace MasterCurrencyConversion
{
    class Program
    {
        private static ITelegramBotClient _botClient;
        private static readonly DataLoader _dataLoader = new DataLoader();
        private const string _ERROR_CASE = "Error request";
        static async Task Main(string[] args)
        {
            _botClient = new TelegramBotClient("1169382738:AAHoo029fBJkoQNyIAL0i02BfFJJMIfpT8s");
            var me = _botClient.GetMeAsync().Result;
            _botClient.OnMessage += Bot_OnMessage;
            _botClient.StartReceiving();
            Console.WriteLine(me.Username);
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
            _botClient.StopReceiving();
        }
        //@//////////////////////////////////////////////////////////////////////////////////////////////////////////////
        static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            StartCommand startCommand = new StartCommand();
            await startCommand.Execute(e.Message, _botClient);
            Telegram.Bot.Types.Message msg = e.Message;
            if (msg.Text == null || msg.Type != MessageType.Text)
            {
                return;
            }

            string date = GetOnlyDate(e.Message.Text.ToString());
            string currency = GetOnlyCurrency(e.Message.Text.ToString());
            if (CheckDate(date))
            {
                await _dataLoader.GetDataPrivateBank(e.Message.Text.ToString(), currency);
                if (CheckCurrency())
                {
                    PrintMessage(e, _dataLoader.GetCurrency());
                }
                else
                {
                    PrintMessage(e, _ERROR_CASE);
                }
            }
            else
            {
                if (date != null && !date.Equals("Start", StringComparison.InvariantCultureIgnoreCase))
                {
                    PrintMessage(e, _ERROR_CASE);
                }
            }
        }
        //@//////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private static async void PrintMessage(MessageEventArgs e, string message)
        {
            await _botClient.SendTextMessageAsync(
                    chatId: e.Message.Chat,
                    text: message
                    );
        }
        //@//////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private static string GetOnlyDate(string str)
        {
            if (str.Equals("Start", StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }
            string[] result = str.Split(' ');
            return result[0];
        }
        //@//////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private static string GetOnlyCurrency(string str)
        {
            if (str.Equals("Start", StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }
            string[] result = str.Split(' ');
            if (result.Length < 2) { return null; }
            return result[1];
        }
        //@//////////////////////////////////////////////////////////////////////////////////////////////////////////////        
        private static bool CheckDate(string date)
        {
            DateTime parsed;
            bool valid = DateTime.TryParseExact(date, "mm.dd.yyyy",
                                    CultureInfo.InvariantCulture,
                                    DateTimeStyles.None,
                                    out parsed);
            return valid;
        }
        //@//////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private static bool CheckCurrency()
        {
            string currency = _dataLoader.GetCurrency();
            if (currency == null)
            {
                return false;
            }
            return true;
        }
    }
}


