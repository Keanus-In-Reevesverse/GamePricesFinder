using GamePriceFinder.Handlers;
using GamePriceFinder.MVC.Controllers.Finders;

namespace GamePriceFinder.MVC.Controllers
{
    public class SearchController
    {
        public SearchController(SteamController steamFinder,
            EpicController epicFinder,
            NuuvemController nuuvemFinder,
            PlaystationController playstationStoreFinder,
            MicrosoftController microsoftFinder)
        {
            SteamFinder = steamFinder;
            EpicFinder = epicFinder;
            NuuvemFinder = nuuvemFinder;
            PlaystationStoreFinder = playstationStoreFinder;
            MicrosoftFinder = microsoftFinder;
        }

        public SteamController SteamFinder { get; }
        public EpicController EpicFinder { get; }
        public NuuvemController NuuvemFinder { get; }
        public PlaystationController PlaystationStoreFinder { get; }
        public MicrosoftController MicrosoftFinder { get; set; }

        public async Task<List<DatabaseEntitiesHandler>> GetPrices(string gameName, int id)
        {
            var steamEntities = await SteamFinder.GetPrice(string.Empty, id);

            var epicEntities = await EpicFinder.GetPrice(gameName);

            var nuuvemEntities = await NuuvemFinder.GetPrice(gameName);

            var psnEntities = await PlaystationStoreFinder.GetPrice(gameName);

            var xboxEntities = await MicrosoftFinder.GetPrice(gameName.Replace(" ", "+"));

            if (epicEntities != null)
            {
                steamEntities?.AddRange(epicEntities);
            }

            if (nuuvemEntities != null)
            {
                steamEntities?.AddRange(nuuvemEntities);
            }

            if (psnEntities != null)
            {
                steamEntities?.AddRange(psnEntities);
            }

            if (xboxEntities != null)
            {
                steamEntities?.AddRange(xboxEntities);
            }


            //foreach (var entity in steamEntities)
            //{
            //    entity.
            //}

            return steamEntities;
        }
    }
}
