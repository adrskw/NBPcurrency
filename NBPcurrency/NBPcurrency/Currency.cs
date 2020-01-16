using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBPcurrency
{
    public class Currency
    {
        public decimal AverageExchangeRate { get; }
        public decimal MinimumExchangeRate { get; }
        public decimal MaximumExchangeRate { get; }
        public decimal StandardDeviation { get; }
        public Dictionary<DateTime, decimal> DatesOfBiggestExchangeRateDifference { get; }

        public Currency(string currency, DateTime startDate, DateTime endDate)
        {
            if (currency == string.Empty)
            {
                throw new ArgumentException("No currency provided");
            }
            else if (Enum.IsDefined(typeof(AvailableCurrencies), currency) == false)
            {
                throw new ArgumentException("Wrong currency provided");
            }
            else if (startDate > DateTime.Now || endDate > DateTime.Now)
            {
                throw new ArgumentException("Given dates are greater than the current date");
            }
            else
            {
                if (startDate > endDate)
                {
                    DateTime temp = endDate;
                    endDate = startDate;
                    startDate = temp;
                }

                AvailableCurrencies enumCurrency = (AvailableCurrencies)Enum.Parse(typeof(AvailableCurrencies), currency);
                ExchangeRates exchangeRates = new ExchangeRates(enumCurrency, startDate, endDate);
            }
        }
    }
}
