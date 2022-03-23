using GamePriceFinder;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//app.MapGet("/", () => "Hello World!");
const int tropicoId = 57690;
const int rSixId = 1922250;
app.MapGet("/", async() =>
{
    var priceSearcher = new PriceSearcher();
    await priceSearcher.GetPrices("");
});

app.Run();
