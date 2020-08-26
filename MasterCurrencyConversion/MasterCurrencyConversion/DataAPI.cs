using System;
using System.Collections.Generic;

namespace MasterCurrencyConversion
{
    public class ExchangeRate
    {
        public string baseCurrency { get; set; }
        public string currency { get; set; }
        public double saleRateNB { get; set; }
        public double purchaseRateNB { get; set; }
        public double? saleRate { get; set; }
        public double? purchaseRate { get; set; }
    }

    public class DataAPI
    {
        public string date { get; set; }
        public string bank { get; set; }
        public int baseCurrency { get; set; }
        public string baseCurrencyLit { get; set; }
        public List<ExchangeRate> exchangeRate { get; set; }
    }
}