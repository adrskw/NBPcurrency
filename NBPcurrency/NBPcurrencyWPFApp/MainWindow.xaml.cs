using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NBPcurrency;

namespace NBPcurrencyWPFApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonDownloadData_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ComboBoxItem selectedItem = ComboBoxCurrencyCode.SelectedItem as ComboBoxItem;
                string currencyCode = selectedItem.Content.ToString();

                DateTime startDate = DateTime.Parse(TextBoxStartDate.Text);
                DateTime endDate = DateTime.Parse(TextBoxEndDate.Text);

                Currency currency = new Currency(currencyCode, startDate, endDate);

                PrintResultData(currency);

                GridResults.Visibility = Visibility.Visible;
            }
            catch (FormatException)
            {
                MessageBox.Show("Invalid date format", "Error occured", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (WebException)
            {
                MessageBox.Show("An error occured during downloading data", "Error occured", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error occured", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Wyświetl rezultaty dla danej waluty
        /// </summary>
        /// <param name="currency">obiekt waluty</param>
        private void PrintResultData(Currency currency)
        {
            TextBoxStartDate.Text = currency.StartDate.ToString("yyyy-MM-dd");
            TextBoxEndDate.Text = currency.EndDate.ToString("yyyy-MM-dd");

            TextBlockAverageBuyExchangeRate.Text = string.Format($"{currency.AverageBuyExchangeRate:0.0000}");
            TextBlockStandardDeviationBuyExchangeRate.Text = string.Format($"{currency.StandardDeviationBuyExchangeRate:0.0000}");
            TextBlockMinimumBuyExchangeRate.Text = currency.MinimumBuyExchangeRate.ToString();
            TextBlockMaximumBuyExchangeRate.Text = currency.MaximumBuyExchangeRate.ToString();
            TextBlockBiggestDifferencesOfBuyExchangeRate.Text = "";

            SortedDictionary<DateTime, decimal> DatesOfBiggestBuyDifferences = currency.DatesOfBiggestBuyExchangeRateDifference;

            foreach (KeyValuePair<DateTime, decimal> date in DatesOfBiggestBuyDifferences)
            {
                TextBlockBiggestDifferencesOfBuyExchangeRate.Text += date.Key.ToString("dd.MM.yyyy") + ": " + date.Value + Environment.NewLine;
            }

            TextBlockAverageSellExchangeRate.Text = string.Format($"{currency.AverageSellExchangeRate:0.0000}");
            TextBlockStandardDeviationSellExchangeRate.Text = string.Format($"{currency.StandardDeviationSellExchangeRate:0.0000}");
            TextBlockMinimumSellExchangeRate.Text = currency.MinimumSellExchangeRate.ToString();
            TextBlockMaximumSellExchangeRate.Text = currency.MaximumSellExchangeRate.ToString();
            TextBlockBiggestDifferencesOfSellExchangeRate.Text = "";

            SortedDictionary<DateTime, decimal> DatesOfBiggestSellDifferences = currency.DatesOfBiggestSellExchangeRateDifference;

            foreach (KeyValuePair<DateTime, decimal> date in DatesOfBiggestSellDifferences)
            {
                TextBlockBiggestDifferencesOfSellExchangeRate.Text += date.Key.ToString("dd.MM.yyyy") + ": " + date.Value + Environment.NewLine;
            }
        }
    }
}
