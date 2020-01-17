using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace NBPcurrency
{
    class ExchangeRates
    {
        public AvailableCurrencies CurrencyCode { get; }
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }
        public List<decimal> BuyPrices { get => dateBuyPrices.Values.ToList(); }
        public List<decimal> SellPrices { get => dateSellPrices.Values.ToList(); }
        public SortedDictionary<DateTime, decimal> dateBuyPrices = new SortedDictionary<DateTime, decimal>();
        public SortedDictionary<DateTime, decimal> dateSellPrices = new SortedDictionary<DateTime, decimal>();

        private Dictionary<int, string> dirFilesContent = new Dictionary<int, string>();

        public ExchangeRates(AvailableCurrencies currencyCode, DateTime startDate, DateTime endDate)
        {
            CurrencyCode = currencyCode;
            StartDate = startDate;
            EndDate = endDate;

            List<DateTime> allDates = GetDateTimesBetweenDates();

            for (int i = 0; i < allDates.Count; i++)
            {
                ProcessDate(allDates[i]);
            }

            while (dateBuyPrices.Count == 0)
            {
                StartDate = StartDate.AddDays(-1);
                EndDate = EndDate.AddDays(1);
                ProcessDate(StartDate);
                ProcessDate(EndDate);
            }
        }

        public SortedDictionary<DateTime, decimal> FindBiggestDifference(SortedDictionary<DateTime, decimal> datePriceData)
        {
            SortedDictionary<DateTime, decimal> differenceData = new SortedDictionary<DateTime, decimal>();
            List<DateTime> dates = datePriceData.Keys.ToList();

            if (dates.Count >= 2)
            {
                for (int i = 1; i < dates.Count; i++)
                {
                    differenceData.Add(dates[i], datePriceData[dates[i]] - datePriceData[dates[i - 1]]); 
                }

                decimal maxAbsoluteDifference = differenceData.Values.Max((val) => Math.Abs(val));

                foreach (KeyValuePair<DateTime, decimal> item in differenceData.Where(kvp => Math.Abs(kvp.Value) != maxAbsoluteDifference).ToList())
                {
                    differenceData.Remove(item.Key);
                }
            }
            else if (dates.Count > 0) {
                differenceData.Add(datePriceData.Keys.First(), 0);
            }

            return differenceData;
        }

        private void ProcessDate(DateTime date)
        {
            string dirFileContent = GetDirFile(date.Year);
            string fileName = SearchFileNameForGivenDate(dirFileContent, date);
            SaveExchangeRatesXml(date, fileName);
        }

        private List<DateTime> GetDateTimesBetweenDates()
        {
            List<DateTime> allDates = new List<DateTime>();

            for (DateTime date = StartDate; date <= EndDate; date = date.AddDays(1))
            {
                allDates.Add(date);
            }

            return allDates;
        }

        private string GetDirFile(int year)
        {
            string dirFileContent;
            if (dirFilesContent.ContainsKey(year)) 
            {
                dirFileContent = dirFilesContent[year];
            }
            else
            {
                string url = "https://www.nbp.pl/kursy/xml/dir" + year + ".txt";

                if (year == DateTime.Now.Year)
                {
                    url = "https://www.nbp.pl/kursy/xml/dir.txt";
                }

                WebClient webClient = new WebClient();
                dirFileContent = webClient.DownloadString(url);
                dirFilesContent[year] = dirFileContent;
            }
            
            return dirFileContent;
        }

        private string SearchFileNameForGivenDate(string dirFileContent, DateTime date)
        {
            if (dirFileContent == string.Empty)
            {
                throw new ArgumentException("Downloaded file list is empty");
            }

            Regex regex = new Regex(@"c\d{3}z" + date.ToString("yyMMdd"));
            Match fileNameMatch = regex.Match(dirFileContent);
            string fileName = "";

            if (fileNameMatch.Success)
            {
                fileName = fileNameMatch.Value;
            }

            return fileName;
        }

        private void SaveExchangeRatesXml(DateTime date, string fileName)
        {
            if (fileName == string.Empty)
            {
                return;
            }

            XmlDocument doc = new XmlDocument();

            XmlTextReader reader = new XmlTextReader("http://www.nbp.pl/kursy/xml/" + fileName + ".xml");
            reader.Read();
            doc.Load(reader);

            XmlNode buyPriceNode = doc.SelectSingleNode("/tabela_kursow/pozycja[kod_waluty='" + CurrencyCode.ToString() + "']/kurs_kupna");
            XmlNode sellPriceNode = doc.SelectSingleNode("/tabela_kursow/pozycja[kod_waluty='" + CurrencyCode.ToString() + "']/kurs_sprzedazy");
            
            decimal buyPrice = decimal.Parse(buyPriceNode.InnerText);
            decimal sellPrice = decimal.Parse(sellPriceNode.InnerText);
            
            dateBuyPrices.Add(date, buyPrice);
            dateSellPrices.Add(date, sellPrice);
        }
    }
}
