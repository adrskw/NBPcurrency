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

        private enum AvailableCurrencies
        {
            USD,
            EUR,
            CHF,
            GBP
        };

        public Currency()
        {

        }
    }
}
