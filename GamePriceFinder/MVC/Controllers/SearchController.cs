﻿using GamePriceFinder.Handlers;
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
            var steamEntities = await SteamFinder.GetPrice(string.Empty, id);

            var epicEntities = await EpicFinder.GetPrice(gameName);

            var nuuvemEntities = await NuuvemFinder.GetPrice(gameName);

            var psnEntities = await PlaystationFinder.GetPrice(gameName);

            var xboxEntities = await MicrosoftFinder.GetPrice(gameName.Replace(" ", "+"));

            if (epicEntities != null)
            {
                foreach (var epicEntity in epicEntities)
                {
                    epicEntity.Genre.Description = steamEntities[0].Genre.Description;
                }

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
                foreach (var xboxEntity in xboxEntities)
                {
                    xboxEntity.Genre.Description = steamEntities[0].Genre.Description;
                }

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
