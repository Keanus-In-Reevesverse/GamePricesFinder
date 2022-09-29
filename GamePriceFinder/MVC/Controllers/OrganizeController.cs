using GamePriceFinder.Handlers;

namespace GamePriceFinder.MVC.Controllers
{
    public class OrganizeController
    {
        public OrganizeController()
        {

        }

        private string NormalizeName(string name) => name.Replace(":", "").Replace("-", "").Replace("™", "").ToLowerInvariant();

        internal void JoinByName(List<DatabaseEntitiesHandler> entities)
        {
            var organizedGames = new List<List<DatabaseEntitiesHandler>>();

            for (int i = 0; i < entities.Count; i++)
            {
                var current = entities[i];

                var currentNormalizedName = NormalizeName(current.Game.Name);

                var match = new List<DatabaseEntitiesHandler>();

                match.Add(current);

                for (int j = i + 1; j < entities.Count; j++)
                {
                    var proximity = JaroWinkler.proximity(currentNormalizedName, NormalizeName(entities[j].Game.Name));

                    if (proximity >= 0.85)
                    {
                        match.Add(entities[j]);
                    }
                }
            }
        }
    }
}
