using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBPcurrency
{
    public class Currency
    {
        public decimal AverageBuyExchangeRate { get => exchangeRates.BuyPrices.Average(); }
        public decimal MinimumBuyExchangeRate { get => exchangeRates.BuyPrices.Min(); }
        public decimal MaximumBuyExchangeRate { get => exchangeRates.BuyPrices.Max(); }
        public decimal StandardDeviationBuyExchangeRate
        {
            get
            {
                decimal sumOfSquaresOfDifferences = exchangeRates.BuyPrices.Sum(val => (val - AverageBuyExchangeRate) * (val - AverageBuyExchangeRate));
                return (decimal)Math.Sqrt((double)(sumOfSquaresOfDifferences / exchangeRates.BuyPrices.Count));
            }
        }
        public SortedDictionary<DateTime, decimal> DatesOfBiggestBuyExchangeRateDifference
        {
            get
            {
                return exchangeRates.FindBiggestDifference(exchangeRates.dateBuyPrices);
            }
        }

        public decimal AverageSellExchangeRate { get => exchangeRates.SellPrices.Average(); }
        public decimal MinimumSellExchangeRate { get => exchangeRates.SellPrices.Min(); }
        public decimal MaximumSellExchangeRate { get => exchangeRates.SellPrices.Max(); }
        public decimal StandardDeviationSellExchangeRate
        {
            get
            {
                decimal sumOfSquaresOfDifferences = exchangeRates.SellPrices.Sum(val => (val - AverageSellExchangeRate) * (val - AverageSellExchangeRate));
                return (decimal)Math.Sqrt((double)(sumOfSquaresOfDifferences / exchangeRates.SellPrices.Count));
            }
        }
        public SortedDictionary<DateTime, decimal> DatesOfBiggestSellExchangeRateDifference { get
            {
                return exchangeRates.FindBiggestDifference(exchangeRates.dateSellPrices);
            }
        }

        private ExchangeRates exchangeRates;

        public Currency(string currency, DateTime startDate, DateTime endDate)
        {
            if (currency == string.Empty)
            {
                throw new ArgumentException("No currency provided");
            }
            
            if (Enum.IsDefined(typeof(AvailableCurrencies), currency) == false)
            {
                throw new ArgumentException("Wrong currency provided");
            }
             
            if (startDate > DateTime.Now || endDate > DateTime.Now)
            {
                throw new ArgumentException("Given dates are greater than the current date");
            }

            if (startDate > endDate)
            {
                DateTime temp = endDate;
                endDate = startDate;
                startDate = temp;
            }

            AvailableCurrencies enumCurrency = (AvailableCurrencies)Enum.Parse(typeof(AvailableCurrencies), currency);
            exchangeRates = new ExchangeRates(enumCurrency, startDate, endDate);
        }
    }
}
