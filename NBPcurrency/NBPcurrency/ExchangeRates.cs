using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBPcurrency
{
    class ExchangeRates
    {
        public AvailableCurrencies Currency { get; }
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }

        public ExchangeRates(AvailableCurrencies currency, DateTime startDate, DateTime endDate)
        {
            Currency = currency;
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
