using CurrencyConvertLib.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConvertLib
{

    public sealed class CurrencyConverter : ICurrencyConverter
    {

        #region Constructor
        CurrencyConverter() { }
        private static readonly Lazy<CurrencyConverter> converter = new Lazy<CurrencyConverter>(() => new CurrencyConverter(), true);

        public static CurrencyConverter Instance()
        {
            return converter.Value;
        }

        #endregion

        #region fields & properties
        IEnumerable<Tuple<string, string, double>> rates = null;

        private List<Rate> _rateList = null;
        private List<Rate> rateList
        {
            get
            {
                if (_rateList == null && rates != null)
                    _rateList = rates.Select(m => new Rate(m.Item1, m.Item2, m.Item3)).ToList();
                return _rateList;
            }
        }

        #endregion

        #region private methods

        private Rate findExactReverse(string fromCurrency, string toCurrency)
        {
            // finding inverse convertion rate.
            return rateList.Where(m => m.From == toCurrency && m.To == fromCurrency).FirstOrDefault();
        }

        private Rate findExact(string fromCurrency, string toCurrency)
        {
            // At first find the exist conversion rate.
            return rateList.Where(m => m.From == fromCurrency && m.To == toCurrency).FirstOrDefault();
        }


        /// <summary>
        /// Check if given destination currency exist in our rates.
        /// </summary>
        /// <param name="toCurrency"></param>
        /// <returns></returns>
        private bool checkCurrencyExist(string toCurrency)
        {
            return rates.Where(m => m.Item1 == toCurrency || m.Item2 == toCurrency).Count() > 0;
        }



        private bool hasConfig()
        {
            return rates != null;
        }

        /// <summary>
        /// Get path that create from graph operation.Make chain of rates and convert given currency.
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="pathList"></param>
        /// <returns></returns>
        private double calculatePath(double amount, List<string> pathList)
        {
            for (int i = 0; i < pathList.Count - 1; i++)
            {
                var rate = findExact(pathList[i], pathList[i + 1]);
                if (rate != null)
                    amount = rate.Calculate(amount);
                else
                {
                    rate = findExactReverse(pathList[i], pathList[i + 1]);
                    amount = rate.CalculateReverse(amount);
                }
            }
            return amount;
        }


        /// <summary>
        /// prepare Tuple<string,string> from rate tuples.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Tuple<string, string>> getGraphTuple()
        {
            return rates.Select(m => Tuple.Create(m.Item1, m.Item2));
        }


        private FindRateResult findRatePath(string from, string to, double amount)
        {
            FindRateResult result = new FindRateResult();
            var operation = new GraphOperation<string>
                   (getGraphTuple());
            var pathList = operation.ShortestPath(from, to);
            if (pathList.Count == 0) return result.Error($"There is no path for convert {from} to {to}.");
            return result.Success(calculatePath(amount, pathList));
        }

        #endregion

        public double Convert(string fromCurrency, string toCurrency, double amount)
        {
            if (!hasConfig())
                throw new Exception("There is no configuration. Updating configuration is necessary.");

            if (!checkCurrencyExist(fromCurrency))
                throw new Exception($"There is no rate for {fromCurrency} in configuration..");

            if (!checkCurrencyExist(toCurrency))
                throw new Exception($"There is no rate for {toCurrency} in configuration.");

            //-------------------------------------
            var foundRate = findExact(fromCurrency, toCurrency);

            if (foundRate != null)
                return foundRate.Calculate(amount);
            //-------------------------------------
            foundRate = findExactReverse(fromCurrency, toCurrency);
            if (foundRate != null)
                return foundRate.CalculateReverse(amount);
            //--------------------------------------
            // find path
            var result = findRatePath(fromCurrency, toCurrency, amount);
            if(result.HasError) throw new Exception(result.Message);
            return result.Result;
        }
        public void ClearConfiguration()
        {
            rates = Enumerable.Empty<Tuple<string, string, double>>();
        }




        public void UpdateConfiguration(IEnumerable<Tuple<string, string, double>> conversionRates)
        {
            this.rates = conversionRates;
        }
    }
}
