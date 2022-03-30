using GamePriceFinder;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", async() =>
{
    var priceSearcher = new PriceSearcher();
    var games = await priceSearcher.GetPrices("fifa");

    foreach (var game in games)
    {
        global::System.Console.WriteLine($"Name: {game.Name}.\nStore: {game.Store}.\nPrice: R$ {game.GameData.CurrentPrice}.");
        global::System.Console.WriteLine();
    }
});

app.Run();
