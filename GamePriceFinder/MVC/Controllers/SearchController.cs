using GamePriceFinder.Handlers;
using GamePriceFinder.MVC.Controllers.Finders;

namespace GamePriceFinder.MVC.Controllers
{
    public class SearchController
    {
        private readonly ILogger<SearchController> _logger;

        public SearchController(SteamController steamFinder,
            EpicController epicFinder,
            NuuvemController nuuvemFinder,
            PlaystationController playstationStoreFinder,
            MicrosoftController microsoftFinder,
            ILogger<SearchController> logger)
        {
            SteamFinder = steamFinder;
            EpicFinder = epicFinder;
            NuuvemFinder = nuuvemFinder;
            PlaystationFinder = playstationStoreFinder;
            MicrosoftFinder = microsoftFinder;
            _logger = logger;
        }

        public SteamController SteamFinder { get; }
        public EpicController EpicFinder { get; }
        public NuuvemController NuuvemFinder { get; }
        public PlaystationController PlaystationFinder { get; }
        public MicrosoftController MicrosoftFinder { get; set; }

        public async Task<List<EntitiesHandler>> GetPrices(string gameName, int id)
        {
            List<EntitiesHandler> steamEntities = null;
            if (id != 0)
            {
                steamEntities = await SteamFinder.GetPrice(string.Empty, id);
            }
            else
            {
                steamEntities = new List<EntitiesHandler>();
            }

            var epicEntities = await EpicFinder.GetPrice(gameName);

            var nuuvemEntities = await NuuvemFinder.GetPrice(gameName);

            var psnEntities = await PlaystationFinder.GetPrice(gameName);

            var xboxEntities = await MicrosoftFinder.GetPrice(gameName.Replace(" ", "+"));

            if (id != 0)
            {
                if (epicEntities != null)
                {
                    foreach (var epicEntity in epicEntities)
                    {
                        epicEntity.Genre.Description = steamEntities[0].Genre.Description;
                    }

                    steamEntities?.AddRange(epicEntities);
                }

                if (xboxEntities != null)
                {
                    foreach (var xboxEntity in xboxEntities)
                    {
                        xboxEntity.Genre.Description = steamEntities[0].Genre.Description;
                    }

                    steamEntities?.AddRange(xboxEntities);
                }
            }
            else
            {
                if (epicEntities != null)
                {
                    EntitiesHandler entityToUse = null;

                    if (nuuvemEntities != null && nuuvemEntities.Count > 0)
                    {
                        entityToUse = nuuvemEntities[0];
                    }
                    else if(psnEntities != null && psnEntities.Count > 0)
                    {
                        entityToUse = psnEntities[0];
                    }

                    foreach (var epicEntity in epicEntities)
                    {
                        epicEntity.Genre.Description = entityToUse?.Genre?.Description;
                    }

                    steamEntities?.AddRange(epicEntities);
                }
            }

            if (nuuvemEntities != null)
            {
                steamEntities?.AddRange(nuuvemEntities);
            }

            if (psnEntities != null)
            {
                steamEntities?.AddRange(psnEntities);
            }


            //foreach (var entity in steamEntities)
            //{
            //    entity.
            //}

            return steamEntities;
        }
    }
}
