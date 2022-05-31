namespace CurrencyConvertLib
{
    public class FindRateResult
    {
        public bool HasError { get; set; }
        public string Message { get; set; }

        public double Result { get; set; }
        public FindRateResult Error(string message)
        {
            this.Message = message;
            this.HasError = true;
            return this;
        }
        public FindRateResult Success(double result)
        {
            this.Result = result;
            this.HasError = false;
            return this;
        }
    }
}
