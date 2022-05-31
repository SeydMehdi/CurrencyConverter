namespace CurrencyConvertLib
{
    public class Rate
    {
        public string From { get; set; }
        public string To { get; set; }
        public double RateValue { get; set; }

        public double Calculate(double amount) {
            return amount * this.RateValue;
        }
        public double CalculateReverse(double amount)
        {
            return amount / this.RateValue;
        }

        public Rate(string item1, string item2, double item3)
        {
            From = item1;
            To = item2;
            RateValue = item3;
        }
    }
}
