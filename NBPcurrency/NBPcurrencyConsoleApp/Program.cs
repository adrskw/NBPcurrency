using NBPcurrency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBPcurrencyConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime date = new DateTime(2000, 1, 2);
            Currency currency = new Currency("CHF", date, date);
        }
    }
}
