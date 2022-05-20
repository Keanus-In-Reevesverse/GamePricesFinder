namespace GamePriceFinder.Handlers
{
    public static class PriceHandler
    {
        public static decimal ConvertPriceToDatabaseType(string commaFormattedPrice, int cutString)
        {
            decimal price;
            try
            {
                price = commaFormattedPrice.Length > 0 && commaFormattedPrice.Length >= cutString
                ? Convert.ToDecimal(commaFormattedPrice.Remove(0, cutString))
                : Convert.ToDecimal(commaFormattedPrice);
            }
            catch
            {
                return 0;
            }

            return price;
        }
    }
}
