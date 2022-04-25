namespace GamePriceFinder.Handlers
{
    public class PriceHandler
    {
        public static decimal ConvertPriceToDatabaseType(string commaFormattedPrice, int cutString)
        {
            return Convert.ToDecimal(commaFormattedPrice.Remove(0, cutString));
        }
    }
}
