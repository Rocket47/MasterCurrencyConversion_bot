using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using Telegram.Bot.Args;
using System.Linq;

namespace MasterCurrencyConversion
{
    class DataLoader
    {
        private string result = null;
        public async Task GetDataPrivateBank(string date)
        {
            var httpClient = HttpClientFactory.Create();
            var url = $"https://api.privatbank.ua/p24api/exchange_rates?json&date={date}";
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(url);
            if (httpResponseMessage.StatusCode == HttpStatusCode.OK)
            {
                var content = httpResponseMessage.Content;
                var data = await content.ReadAsAsync<DataAPI>();
                result = GetExchangeRate(SearchUSDValue(data.exchangeRate));
            }
        }

        public  string[] SearchUSDValue(object[] arr)
        {
            List<string[]> listForSearchingUSD = GenerateListFromArr(arr);
            foreach (string[] item in listForSearchingUSD)
            {
                if (item[5].Equals("\"USD\""))
                {
                    return item;
                }
            }
            return null;
        }

        public List<string[]> GenerateListFromArr(object[] arr)
        {
            List<string[]> resultList = new List<string[]>();
            for (int i = 0; i < arr.Length; i++)
            {
                var newArray = arr[i].ToString().Split(" ,".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries).ToArray();
                resultList.Add(newArray);
            }
            return resultList;
        }

        public string GetExchangeRate(string[] arr)
        {
            int indexSaleRate = 0;
            int indexPurchaseRate = 0;
            string result = "";

            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i].Equals("\"saleRate\":"))
                {
                    indexSaleRate = i;
                }
                if (arr[i].Equals("\"purchaseRate\":"))
                {
                    indexPurchaseRate = i;
                }
            }
            result = arr[indexSaleRate] + " " + arr[indexSaleRate + 1] + " " + "\n" + arr[indexPurchaseRate] + " " + arr[indexPurchaseRate + 1];
            result = result.TrimEnd(new char[] { '}' });
            return result;           
        }

        public string GetInfo() => result;       
    }
}
