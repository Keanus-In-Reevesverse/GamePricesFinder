using GamePriceFinder.Handlers;
using GamePriceFinder.MVC.Models;
using GamePriceFinder.MVC.Models.Intefaces;

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

        private void InsertNewGameAndProperties(List<History> histories, EntitiesHandler gameEntity)
        {
            var genreId = _genreRepository.FindOneByName(gameEntity.Genre.Description).GenreId;

            gameEntity.Game.GenreId = genreId;

            _gameRepository.AddOne(gameEntity.Game);

            var insertedGame = _gameRepository.FindOneByName(gameEntity.Game.Name);

            gameEntity.GamePrices.GameId = insertedGame.GameId;

            _gamePricesRepository.AddOne(gameEntity.GamePrices);

            var lowestPriceHistory = GetCheapestPrice(histories);

            lowestPriceHistory.GameId = insertedGame.GameId;

            _historyRepository.AddOne(lowestPriceHistory);
        }

        internal void ManageDatabase(List<List<EntitiesHandler>> organizedGameList)
        {
            if (!_gameRepository.FindAll().Any())
            {
                foreach (var gameList in organizedGameList)
                {
                    var histories = gameList.Select(gList => gList.History).ToList();

                    foreach (var gameEntity in gameList)
                    {
                        //TODO AJUSTAR TAMANHO DO CAMPO DE VIDEO
                        //TODO PEGAR PUBLISHER

                        if (string.IsNullOrEmpty(gameEntity.Game.Publisher))
                        {
                            gameEntity.Game.Publisher = "iudhfiaufhg";
                        }

                        gameEntity.Game.Video = "isudhfiuasdhf";

                        InsertNewGameAndProperties(histories, gameEntity);

                        break;
                    }
                }
            }
            else
            {
                foreach (var gameList in organizedGameList)
                {
                    var histories = gameList.Select(gList => gList.History).ToList();

                    foreach (var gameEntity in gameList)
                    {
                        var game = _gameRepository.FindOneByName(gameEntity.Game.Name);

                        if (game != null)
                        {
                            var gamePrices = _gamePricesRepository.FindAll();

                            var currentGamePrice = gamePrices.FirstOrDefault(
                                gPrices => gPrices.GameId == game.GameId && gPrices.StoreId == gameEntity.GamePrices.StoreId);

                            currentGamePrice.CurrentPrice = gameEntity.GamePrices.CurrentPrice;

                            currentGamePrice.Link = gameEntity.GamePrices.Link;

                            _gamePricesRepository.Update(currentGamePrice);

                            var lowestPriceHistory = GetCheapestPrice(histories);

                            lowestPriceHistory.GameId = game.GameId;

                            _historyRepository.AddOne(lowestPriceHistory);
                            
                            break;
                        }
                        else
                        {
                            InsertNewGameAndProperties(histories, gameEntity);
                            break;
                        }
                    }
                }
            }
        }
    }
}
