using GamePriceFinder;
using GamePriceFinder.Database;
using GamePriceFinder.Finders;
using GamePriceFinder.Intefaces;
using GamePriceFinder.Models;
using GamePriceFinder.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();


var connectionString = builder.Configuration.GetSection("MySqlConnection:MySqlConnectionString").Value;

builder.Services.AddTransient<IRepository<Genre>, GenreRepository>();
builder.Services.AddTransient<IRepository<Game>, GameRepository>();
builder.Services.AddTransient<IRepository<History>, HistoryRepository>();
builder.Services.AddTransient<PriceSearcher>();
builder.Services.AddTransient<SteamFinder>();
builder.Services.AddTransient<EpicFinder>();
builder.Services.AddTransient<NuuvemFinder>();
builder.Services.AddTransient<PlaystationStoreFinder>();
builder.Services.AddTransient<MicrosoftFinder>();

builder.Services.AddScoped<DbContextOptions<DbContext>>();
builder.Services.
    AddDbContext<DatabaseContext>(opts => opts.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

var app = builder.Build();
//app.Urls.Add("http://localhost:5000");
app.MapGet("/", async(
    [FromServices] IRepository<Genre> genreRepository,
    [FromServices] IRepository<History> historyRepository,
    [FromServices] IRepository<Game> gameRepository,
    [FromServices] PriceSearcher priceSearcher) =>
{
    var gameName = "FOR HONOR";

    var gameId = gameRepository.FindOne(gameName).GameId;

    var entities = await priceSearcher.GetPrices(gameName);

    foreach (var entity in entities)
    {
        global::System.Console.WriteLine();
        global::System.Console.WriteLine(string.Concat("Store: ", entity.History.StoreName), ".");
        global::System.Console.WriteLine(string.Concat("Name: ", entity.Game.Name), ".");
        global::System.Console.WriteLine(string.Concat("Current price: R$ ", entity.GamePrices.CurrentPrice), ".");
    }

    foreach (var entity in entities)
    {
        entity.History.GameId = gameId;
        historyRepository.AddOne(entity.History);
    }

    
    var gamePricesRepository = new GamePricesRepository();
});

app.Run();


