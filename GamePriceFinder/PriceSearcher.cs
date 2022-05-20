using GamePriceFinder.Finders;
using GamePriceFinder.Handlers;

namespace GamePriceFinder
{
    public class PriceSearcher
    {
        public PriceSearcher(SteamFinder steamFinder, EpicFinder epicFinder, NuuvemFinder nuuvemFinder, PlaystationStoreFinder playstationStoreFinder)
        {
            SteamFinder = steamFinder;
            EpicFinder = epicFinder;
            NuuvemFinder = nuuvemFinder;
            PlaystationStoreFinder = playstationStoreFinder;
        }

        public SteamFinder SteamFinder { get; }
        public EpicFinder EpicFinder { get; }
        public NuuvemFinder NuuvemFinder { get; }
        public PlaystationStoreFinder PlaystationStoreFinder { get; }

        public async Task<List<DatabaseEntitiesHandler>> GetPrices(string gameName)
        {
            var steamEntities = await SteamFinder.GetPrice(string.Empty);

            var epicEntities = await EpicFinder.GetPrice(gameName);

            var nuuvemEntities = await NuuvemFinder.GetPrice(gameName);

            var psnEntities = await PlaystationStoreFinder.GetPrice(gameName);

            steamEntities.AddRange(epicEntities);

            steamEntities.AddRange(nuuvemEntities);

            steamEntities.AddRange(psnEntities);

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
