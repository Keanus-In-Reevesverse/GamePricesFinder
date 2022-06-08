namespace GamePriceFinder.Handlers
{
    /// <summary>
    /// Static class for price conversion.
    /// </summary>
    public static class PriceHandler
    {
        /// <summary>
        /// Converts price from string to decimal.
        /// </summary>
        /// <param name="commaFormattedPrice"></param>
        /// <param name="cutString"></param>
        /// <returns></returns>
        public static decimal ConvertPriceToDatabaseType(string commaFormattedPrice, int cutString)
        {
            decimal price;
            try
            {
                var convertedPrice = commaFormattedPrice.Contains(".") ? commaFormattedPrice.Replace(".", ",") : commaFormattedPrice;
                price = commaFormattedPrice.Length > 0 && commaFormattedPrice.Length >= cutString
                ? Convert.ToDecimal(convertedPrice.Remove(0, cutString))
                : Convert.ToDecimal(convertedPrice);
            }
            catch
            {
                return 0;
            }

            return price;
        }
    }
}
