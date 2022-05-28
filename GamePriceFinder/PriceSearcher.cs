using GamePriceFinder.Finders;
using GamePriceFinder.Handlers;

namespace GamePriceFinder
{
    /// <summary>
    /// Unificates the price finders for each store.
    /// </summary>
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

        /// <summary>
        /// Steam price finder.
        /// </summary>
        public SteamFinder SteamFinder { get; }
        /// <summary>
        /// Epic Games price finder.
        /// </summary>
        public EpicFinder EpicFinder { get; }
        /// <summary>
        /// Nuuvem price finder.
        /// </summary>
        public NuuvemFinder NuuvemFinder { get; }
        /// <summary>
        /// Playstation Store price finder.
        /// </summary>
        public PlaystationStoreFinder PlaystationStoreFinder { get; }
        /// <summary>
        /// Xbox store price finder.
        /// </summary>
        public MicrosoftFinder MicrosoftFinder { get; set; }

        /// <summary>
        /// Retrieves prices of each store.
        /// </summary>
        /// <param name="gameName"></param>
        /// <returns></returns>
        public async Task<List<DatabaseEntitiesHandler>> GetPrices(string gameName)
        {
            var steamEntities = await SteamFinder.GetPrice(string.Empty);

            var epicEntities = await EpicFinder.GetPrice(gameName);

            var nuuvemEntities = await NuuvemFinder.GetPrice(gameName);

            var psnEntities = await PlaystationStoreFinder.GetPrice(gameName);

            var xboxEntities = await MicrosoftFinder.GetPrice(gameName.Replace(" ", "+"));
            steamEntities.AddRange(epicEntities);

            steamEntities.AddRange(nuuvemEntities);

            steamEntities.AddRange(psnEntities);

            steamEntities.AddRange(xboxEntities);

            //foreach (var entity in steamEntities)
            //{
            //    entity.
            //}

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
