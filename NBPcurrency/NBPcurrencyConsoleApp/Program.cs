using NBPcurrency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace NBPcurrencyConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 3)
            {
                try
                {
                    string currencyCode = args[0];
                    DateTime startDate = DateTime.Parse(args[1]);
                    DateTime endDate = DateTime.Parse(args[2]);

                    Currency currency = new Currency(currencyCode, startDate, endDate);

                    PrintInformation(currency);
                }
                catch(FormatException)
                {
                    Console.WriteLine("Invalid date format");
                }
                catch(WebException)
                {
                    Console.WriteLine("An error occured during downloading data");
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                Console.WriteLine("Only 3 parameters can be specified");
            }

            Console.ReadKey();
        }

        /// <summary>
        /// Print information about currency
        /// </summary>
        /// <param name="currency"></param>
        private static void PrintInformation(Currency currency)
        {
            Console.WriteLine($"Currency: {currency.CurrencyCode}");
            Console.WriteLine($"Starting Date: {currency.StartDate.ToString("yyyy-MM-dd")}");
            Console.WriteLine($"Ending Date: {currency.EndDate.ToString("yyyy-MM-dd")}");
            Console.WriteLine();
            Console.WriteLine("Buy Exchange Rates:");
            Console.WriteLine($"    Average: {currency.AverageBuyExchangeRate:0.0000}");
            Console.WriteLine($"    Standard Deviation: {currency.StandardDeviationBuyExchangeRate:0.0000}");
            Console.WriteLine($"    Minimum: {currency.MinimumBuyExchangeRate}");
            Console.WriteLine($"    Maximum: {currency.MaximumBuyExchangeRate}");
            Console.WriteLine($"    Dates of the greatest exchange rate differences:");
            SortedDictionary<DateTime, decimal> DatesOfBiggestBuyDifferences = currency.DatesOfBiggestBuyExchangeRateDifference;

            foreach (KeyValuePair<DateTime, decimal> date in DatesOfBiggestBuyDifferences)
            {
                Console.WriteLine($"      {date.Key.ToString("dd.MM.yyyy")}: {date.Value}");
            }

            Console.WriteLine();
            Console.WriteLine("Sell Exchange Rates:");
            Console.WriteLine($"    Average: {currency.AverageSellExchangeRate:0.0000}");
            Console.WriteLine($"    Standard Deviation: {currency.StandardDeviationSellExchangeRate:0.0000}");
            Console.WriteLine($"    Minimum: {currency.MinimumSellExchangeRate}");
            Console.WriteLine($"    Maximum: {currency.MaximumSellExchangeRate}");
            Console.WriteLine($"    Dates of the greatest exchange rate differences:");
            SortedDictionary<DateTime, decimal> DatesOfBiggestSellDifferences = currency.DatesOfBiggestSellExchangeRateDifference;

            foreach (KeyValuePair<DateTime, decimal> date in DatesOfBiggestSellDifferences)
            {
                Console.WriteLine($"      {date.Key.ToString("dd.MM.yyyy")}: {date.Value}");
            }

        }
    }
}
