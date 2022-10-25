using GamePriceFinder.Handlers;

namespace GamePriceFinder.MVC.Controllers
{
    public class OrganizeController
    {
        private readonly ILogger<OrganizeController> _logger;

        public OrganizeController(
            ILogger<OrganizeController> logger)
        {
            _logger = logger;
        }

        private string NormalizeName(string name) => name.Replace(":", "").Replace("-", "").Replace("™", "").ToLowerInvariant();

        internal void JoinByName(List<DatabaseEntitiesHandler> entities)
        {
            var organizedGames = new List<List<DatabaseEntitiesHandler>>();

            var ignoreComparison = new List<int>();

            for (int i = 0; i < entities.Count; i++)
            {
                var stop = false;
                foreach (var org in organizedGames)
                {
                    if (org.Any(item => item.Game.Name.Equals(entities[i].Game.Name)))
                    {
                        stop = true;
                        break;
                    }
                }

                if (stop)
                {
                    continue;
                }

                var current = entities[i];

                var currentNormalizedName = NormalizeName(current.Game.Name);

                var match = new List<DatabaseEntitiesHandler>();

                match.Add(current);

                for (int j = i + 1; j < entities.Count; j++)
                {
                    if (ignoreComparison.Any(index => index == j))
                    {
                        continue;
                    }

                    var proximity = JaroWinkler.proximity(currentNormalizedName, NormalizeName(entities[j].Game.Name));

                    if (proximity >= 0.9)
                    {
                        match.Add(entities[j]);
                        ignoreComparison.Add(j);
                    }
                }

                organizedGames.Add(match);
            }
        }
    }
}
