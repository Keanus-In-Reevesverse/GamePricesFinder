using GamePriceFinder.Finders;
using GamePriceFinder.Handlers;

namespace GamePriceFinder
{
    public class PriceSearcher
    {
        public PriceSearcher(SteamFinder steamFinder, 
            EpicFinder epicFinder, 
            NuuvemFinder nuuvemFinder, 
            PlaystationStoreFinder playstationStoreFinder,
            MicrosoftFinder microsoftFinder)
        {
            SteamFinder = steamFinder;
            EpicFinder = epicFinder;
            NuuvemFinder = nuuvemFinder;
            PlaystationStoreFinder = playstationStoreFinder;
            MicrosoftFinder = microsoftFinder;
        }

        public SteamFinder SteamFinder { get; }
        public EpicFinder EpicFinder { get; }
        public NuuvemFinder NuuvemFinder { get; }
        public PlaystationStoreFinder PlaystationStoreFinder { get; }

        public MicrosoftFinder MicrosoftFinder { get; set; }

        public async Task<List<DatabaseEntitiesHandler>> GetPrices(string gameName)
        {
            var steamEntities = await SteamFinder.GetPrice(string.Empty);

            var epicEntities = await EpicFinder.GetPrice(gameName);

            var nuuvemEntities = await NuuvemFinder.GetPrice(gameName);

            var psnEntities = await PlaystationStoreFinder.GetPrice(gameName);

            var xboxEntities = await MicrosoftFinder.GetPrice(gameName.Replace(" ", "%20"));

            steamEntities.AddRange(epicEntities);

            steamEntities.AddRange(nuuvemEntities);

            steamEntities.AddRange(psnEntities);

            steamEntities.AddRange(xboxEntities);

            return steamEntities;
        }

        //private Task FillGameInformation(ref Game game, string commaFormattedPrice, int cutPrice)
        //{
        //    var currentPrice = PriceHandler.ConvertPriceToDatabaseType(commaFormattedPrice.Replace(".", ","), cutPrice);

        //    game.GameData.CurrentPrice = currentPrice;

        //    game.History.Price = currentPrice;

        //    game.History.ChangeDate = DateTime.Now.ToShortDateString();

        //    return Task.CompletedTask;
        //}
    }
}
