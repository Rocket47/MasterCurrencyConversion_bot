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
        private static ITelegramBotClient botClient;        
        private static DataLoader dataLoader = new DataLoader();
        static async Task Main(string[] args)
        {
            botClient = new TelegramBotClient("1169382738:AAHoo029fBJkoQNyIAL0i02BfFJJMIfpT8s");
            var me = botClient.GetMeAsync().Result;           
            botClient.OnMessage += Bot_OnMessage;
            botClient.StartReceiving();
            Console.WriteLine(me.Username);
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
            botClient.StopReceiving();
        }
        //@//////////////////////////////////////////////////////////////////////////////////////////////////////////////
        static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            StartCommand startCommand = new StartCommand();
            await startCommand.Execute(e.Message, botClient);
            Telegram.Bot.Types.Message msg = e.Message;
            if (msg.Text == null || msg.Type != MessageType.Text)
            {                
                return;
            }                        
        
            string date = GetOnlyDate(e.Message.Text.ToString());
            string currency = GetOnlyCurrency(e.Message.Text.ToString());

            if (CheckDate(date)) 
            {
                await dataLoader.GetDataPrivateBank(e.Message.Text.ToString(), currency);
                if (CheckCurrency())
                {
                    PrintMessage(e, dataLoader.GetCurrency());
                }
            }           
            if ((!CheckDate(date)) && (!CheckCurrency()) && (!e.Message.Text.Equals("Start")))
            {
                PrintMessage(e, "Invalid format");
                return;
            }
        }
        //@//////////////////////////////////////////////////////////////////////////////////////////////////////////////
        static async void PrintMessage(MessageEventArgs e, string message)
        {
            await botClient.SendTextMessageAsync(
                    chatId: e.Message.Chat,
                    text: message
                    );
        }
        //@//////////////////////////////////////////////////////////////////////////////////////////////////////////////
        static string GetOnlyDate(string str) 
        {
            if (!str.Equals("Start"))
            {
                string[] result = str.Split(' ');
                return result[0];
            }
            return null;
        }
        //@//////////////////////////////////////////////////////////////////////////////////////////////////////////////
        static string GetOnlyCurrency(string str)
        {
            if (!str.Equals("Start"))
            {
                string[] result = str.Split(' ');
                if (result.Length < 2) { return null; }
                return result[1];
            }
            return null;
        }
        //@//////////////////////////////////////////////////////////////////////////////////////////////////////////////        
        static bool CheckDate(string date)
        {           
            DateTime parsed;
            bool valid = DateTime.TryParseExact(date, "mm.dd.yyyy",
                                    CultureInfo.InvariantCulture,
                                    DateTimeStyles.None,
                                    out parsed);            
            return valid;
        }
        //@//////////////////////////////////////////////////////////////////////////////////////////////////////////////
        static bool CheckCurrency()
        {
            string currency = dataLoader.result;
            if (currency == null)
            {
                return false;
            }
            return true;
        }
    }
}
