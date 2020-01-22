using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBPcurrency
{
    public class Currency
    {
        /// <summary>
        /// Kod waluty
        /// </summary>
        public AvailableCurrencies CurrencyCode { get; }

        /// <summary>
        /// Data początku zbierania danych
        /// </summary>
        public DateTime StartDate { get; }

        /// <summary>
        /// Data końca zbierania danych
        /// </summary>
        public DateTime EndDate { get; }

        /// <summary>
        /// Średni kurs kupna
        /// </summary>
        public decimal AverageBuyExchangeRate { get => exchangeRates.BuyPrices.Average(); }

        /// <summary>
        /// Minimalny kurs kupna
        /// </summary>
        public decimal MinimumBuyExchangeRate { get => exchangeRates.BuyPrices.Min(); }

        /// <summary>
        /// Maksymalny kurs kupna
        /// </summary>
        public decimal MaximumBuyExchangeRate { get => exchangeRates.BuyPrices.Max(); }

        /// <summary>
        /// Odchylenie standardowe kursu kupna
        /// </summary>
        public decimal StandardDeviationBuyExchangeRate
        {
            get
            {
                decimal sumOfSquaresOfDifferences = exchangeRates.BuyPrices.Sum(val => (val - AverageBuyExchangeRate) * (val - AverageBuyExchangeRate));
                return (decimal)Math.Sqrt((double)(sumOfSquaresOfDifferences / exchangeRates.BuyPrices.Count));
            }
        }

        /// <summary>
        /// Daty z największą różnicą kursów kupna
        /// </summary>
        public SortedDictionary<DateTime, decimal> DatesOfBiggestBuyExchangeRateDifference
        {
            get
            {
                return exchangeRates.FindBiggestDifference(exchangeRates.dateBuyPrices);
            }
        }


        /// <summary>
        /// Średni kurs sprzedaży
        /// </summary>
        public decimal AverageSellExchangeRate { get => exchangeRates.SellPrices.Average(); }

        /// <summary>
        /// Minimalny kurs sprzedaży
        /// </summary>
        public decimal MinimumSellExchangeRate { get => exchangeRates.SellPrices.Min(); }

        /// <summary>
        /// Maksymalny kurs sprzedaży
        /// </summary>
        public decimal MaximumSellExchangeRate { get => exchangeRates.SellPrices.Max(); }

        /// <summary>
        /// Odchylenie standardowe kursu sprzedaży
        /// </summary>
        public decimal StandardDeviationSellExchangeRate
        {
            get
            {
                decimal sumOfSquaresOfDifferences = exchangeRates.SellPrices.Sum(val => (val - AverageSellExchangeRate) * (val - AverageSellExchangeRate));
                return (decimal)Math.Sqrt((double)(sumOfSquaresOfDifferences / exchangeRates.SellPrices.Count));
            }
        }

        /// <summary>
        /// Daty z największą różnicą kursów sprzedaży
        /// </summary>
        public SortedDictionary<DateTime, decimal> DatesOfBiggestSellExchangeRateDifference 
        {
            get
            {
                return exchangeRates.FindBiggestDifference(exchangeRates.dateSellPrices);
            }
        }

        private readonly ExchangeRates exchangeRates;
        private readonly DateTime startDateOfCollectingData = new DateTime(2002, 1, 2);

        /// <summary>
        /// Informacje o walucie z danego okresu
        /// </summary>
        /// <param name="currencyCode">kod waluty (USD, EUR, CHF, GBP)</param>
        /// <param name="startDate">data początku zbierania danych</param>
        /// <param name="endDate">data końca zbierania danych</param>
        public Currency(string currencyCode, DateTime startDate, DateTime endDate)
        {
            if (currencyCode == string.Empty)
            {
                throw new ArgumentException("No currency provided");
            }
            
            if (Enum.IsDefined(typeof(AvailableCurrencies), currencyCode) == false)
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

            if (startDate < startDateOfCollectingData)
            {
                throw new ArgumentException("Given starting date is less than the beginning date of data collection (2002-01-02)");
            }

            StartDate = startDate;
            EndDate = endDate;
            CurrencyCode = (AvailableCurrencies)Enum.Parse(typeof(AvailableCurrencies), currencyCode);
            exchangeRates = new ExchangeRates(CurrencyCode, StartDate, EndDate);
        }
    }
}
