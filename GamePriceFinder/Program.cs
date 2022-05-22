using GamePriceFinder;
using GamePriceFinder.Finders;
using GamePriceFinder.Repositories;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
//app.Urls.Add("http://localhost:5000");

app.MapGet("/", async() =>
{
    var storesHandler = new PriceSearcher(new SteamFinder(), 
        new EpicFinder(),new NuuvemFinder(), new PlaystationStoreFinder(), new MicrosoftFinder());

    var entities = await storesHandler.GetPrices("for honor");

    foreach (var entity in entities)
    {
        global::System.Console.WriteLine();
        global::System.Console.WriteLine(string.Concat("Store: ", entity.History.StoreName), ".");
        global::System.Console.WriteLine(string.Concat("Name: ", entity.Game.Name), ".");
        global::System.Console.WriteLine(string.Concat("Current price: R$ ", entity.GamePrices.CurrentPrice), ".");
    }

    var gameRepository = new GameRepository();
    var gamePricesRepository = new GamePricesRepository();
    var historyRepository = new HistoryRepository();
});

app.Run();
