namespace GamePriceFinder.Handlers
{
    public class PriceHandler
    {
        private string _priceFormatted;

        public string PriceFormatted
        {
            get { return _priceFormatted; }
            set { _priceFormatted = value; }
        }

        private int _price;

        public int Price
        {
            get
            {
                if (_price == 0)
                {
                    throw new Exception("Price is 0");
                }
                return _price;
            }
            set { _price = value; }
        }

        public decimal ConvertPriceToDatabaseType(int cutString)
        {
            return Convert.ToDecimal(PriceFormatted.Remove(0, cutString));
        }
    }
}
