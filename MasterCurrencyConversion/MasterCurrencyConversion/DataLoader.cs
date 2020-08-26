using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using System;

namespace MasterCurrencyConversion
{
    class DataLoader
    {
        private string result = null;
        //@/////////////////////////////////////////////////////////////////////////////////////
        public async Task GetDataPrivateBank(string date, string currency)
        {
            var httpClient = HttpClientFactory.Create();
            var url = $"https://api.privatbank.ua/p24api/exchange_rates?json&date={date}";
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(url);
            if (httpResponseMessage.StatusCode == HttpStatusCode.OK)
            {
                var content = httpResponseMessage.Content;
                var data = await content.ReadAsAsync<DataAPI>();
                if (data == null)
                {
                    result = "Error request.";
                }
                else
                {                    
                    result = GetExchangeRate(data.exchangeRate, currency);
                }
            }
        }
        //@/////////////////////////////////////////////////////////////////////////////////////
        public string GetCurrency() => result;

        //@/////////////////////////////////////////////////////////////////////////////////////
        private string GetExchangeRate(List<ExchangeRate> arr, string searchCuurencyValue)
        {
            string result = ""; 
            if (searchCuurencyValue == null)
            {
                result =  null;
                return result;
            }
            ExchangeRate item = arr.Find(x => x.currency != null && x.currency.Equals(searchCuurencyValue, StringComparison.InvariantCultureIgnoreCase));
            if (item == null)
            {
                result = "Error request";
                return result;
            }
            result = $"saleRateNB = {item.saleRateNB}\npurchaseRateNB = {item.purchaseRateNB}";
            return result;
        }       
    }
}
