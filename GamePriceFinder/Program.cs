using GamePriceFinder;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", async() =>
{
    var priceSearcher = new PriceSearcher();
    var games = await priceSearcher.GetPrices("for honor");

    foreach (var game in games)
    {
        global::System.Console.WriteLine(game);
    }
});

app.Run();
