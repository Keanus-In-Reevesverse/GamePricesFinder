using GamePriceFinder.Handlers;
using GamePriceFinder.MVC.Models;
using GamePriceFinder.MVC.Models.Intefaces;
using Windows.Media.Capture;

namespace GamePriceFinder.MVC.Controllers
{
    public class DatabaseController
    {
        private readonly IRepository<Genre> _genreRepository;
        private readonly IRepository<Game> _gameRepository;
        private readonly IRepository<GamePrices> _gamePricesRepository;
        private readonly IRepository<History> _historyRepository;

        public DatabaseController(IRepository<Genre> genreRepository,
                                  IRepository<Game> gameRepository,
                                  IRepository<GamePrices> gamePricesRepository,
                                  IRepository<History> historyRepository)
        {
            _genreRepository = genreRepository;
            _gameRepository = gameRepository;
            _gamePricesRepository = gamePricesRepository;
            _historyRepository = historyRepository;
        }

        private History GetCheapestPrice(List<History> histories)
        {
            var lowestPriceHistory = histories[0];

            foreach (var history in histories)
            {
                if (lowestPriceHistory.Price > history.Price)
                {
                    lowestPriceHistory = history;
                }
            }

            return lowestPriceHistory;
        }

        private void InsertNewGameAndProperties(EntitiesHandler gameEntity, bool gotFirstGame)
        {
            var genreId = _genreRepository.FindOneByName(gameEntity.Genre.Description).GenreId;

            gameEntity.Game.GenreId = genreId;

            if (!gotFirstGame)
            {
                _gameRepository.AddOne(gameEntity.Game);
            }

            var insertedGame = _gameRepository.FindOneByName(gameEntity.Game.Name);

            gameEntity.GamePrices.GameId = insertedGame.GameId;

            _gamePricesRepository.AddOne(gameEntity.GamePrices);

            gameEntity.History.GameIdentifier = insertedGame.GameId;

            _historyRepository.AddOne(gameEntity.History);
        }

        internal void ManageDatabase(List<List<EntitiesHandler>> organizedGameList)
        {
            var allGames = _gameRepository.FindAll();

            if (allGames == null)
            {
                foreach (var gameList in organizedGameList)
                {
                    var gotFirstGame = false;

                    var gameNameToUse = gameList.Select(gList => gList.Game.Name).FirstOrDefault();

                    foreach (var gameEntity in gameList)
                    {
                        //TODO PEGAR PUBLISHER

                        gameEntity.Game.Name = gameNameToUse;

                        if (string.IsNullOrEmpty(gameEntity.Game.Publisher))
                        {
                            gameEntity.Game.Publisher = string.Empty;
                        }

                        InsertNewGameAndProperties(gameEntity, gotFirstGame);

                        gotFirstGame = true;
                    }
                }
            }
            else
            {
                foreach (var gameList in organizedGameList)
                {
                    var gameNames = gameList.Select(g => g.Game.Name);

                    var gameNameToUse = gameNames.FirstOrDefault();

                    var gotFirstGame = false;

                    foreach (var gameEntity in gameList)
                    {
                        gameEntity.Game.Name = gameNameToUse;

                        var exists = false;

                        Game game = null;

                        foreach (var gameName in gameNames)
                        {
                            try
                            {
                                game = _gameRepository.FindOneByName(gameEntity.Game.Name);

                                if (game != null)
                                {
                                    exists = true;
                                    gameNameToUse = gameEntity.Game.Name;
                                    break;
                                }
                            }
                            catch
                            {
                                exists = false;
                                break;
                            }
                        }

                        if (exists)
                        {
                            var gamePrices = _gamePricesRepository.FindAll();

                            var currentGamePrice = gamePrices.FirstOrDefault(
                                gPrices => gPrices.GameId == game.GameId && gPrices.StoreId == gameEntity.GamePrices.StoreId);

                            if (currentGamePrice == null)
                            {
                                gameEntity.GamePrices.GameId = game.GameId;
                                _gamePricesRepository.AddOne(gameEntity.GamePrices);
                            }
                            else
                            {
                                currentGamePrice.CurrentPrice = gameEntity.GamePrices.CurrentPrice;

                                currentGamePrice.Link = gameEntity.GamePrices.Link;

                                _gamePricesRepository.Update(currentGamePrice);
                            }

                            gameEntity.History.GameIdentifier = game.GameId;

                            _historyRepository.AddOne(gameEntity.History);
                        }
                        else
                        {
                            InsertNewGameAndProperties(gameEntity, gotFirstGame);
                            gotFirstGame = true;
                        }
                    }
                }
            }
        }
    }
}
