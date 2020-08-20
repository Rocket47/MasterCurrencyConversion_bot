using System;
using Telegram.Bot;
using Telegram.Bot.Args;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Generic;
using System.Net;
using Telegram.Bot.Requests;
using System.Linq;
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
            if (CheckDate(date))
            {
                await dataLoader.GetDataPrivateBank(e.Message.Text.ToString());
                PrintMessage(e, dataLoader.GetInfo());
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
            string[] result = str.Split(' ');
            return result[0];
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
    }
}
