using GamePriceFinder;
using GamePriceFinder.Database;
using GamePriceFinder.MVC.Controllers;
using GamePriceFinder.MVC.Controllers.Finders;
using GamePriceFinder.MVC.Models;
using GamePriceFinder.MVC.Models.Intefaces;
using GamePriceFinder.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

var connectionString = builder.Configuration.GetSection("MySqlConnection:MySqlConnectionString").Value;

builder.Services.AddTransient<IRepository<Genre>, GenreRepository>();
builder.Services.AddTransient<IRepository<Game>, GameRepository>();
builder.Services.AddTransient<IRepository<GamePrices>, GamePricesRepository>();
builder.Services.AddTransient<IRepository<History>, HistoryRepository>();
builder.Services.AddTransient<SearchController>();
builder.Services.AddTransient<OrganizeController>();
builder.Services.AddTransient<SteamController>();
builder.Services.AddTransient<EpicController>();
builder.Services.AddTransient<NuuvemController>();
builder.Services.AddTransient<PlaystationController>();
builder.Services.AddTransient<MicrosoftController>();

builder.Services.AddLogging(config => config.AddConsole());


builder.Services.AddScoped<DbContextOptions<DbContext>>();
builder.Services.
    AddDbContext<DatabaseContext>(opts => opts.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

var app = builder.Build();

//app.Urls.Add("http://localhost:5000");

app.MapGet("/", async (
    [FromServices] SearchController searchSearcher,
    [FromServices] OrganizeController organizeController,
    [FromServices] ILogger<Program> logger)=>
{
    logger.LogInformation("Searching for games...");
    var gameNames = new List<string>() { "for honor", "for honor starter edition", "for honor standard edition", "for honor marching fire edition",
        "scribblenauts unlimited", "scribblenauts unmasked", "scribblenauts mega pack", "scribblenauts showdown",
        "lego batman 3", "lego batman 3 além de gotham edição luxo", "lego batman 2", "lego batman",
        "the witcher 3", "the witcher 3 game of the year edition", "the witcher 3 wild hunt complete edition", "the witcher 2",
        "nioh complete edition", "thronebreaker the witcher tales" };

    //var gameNames = new List<string>() { "for honor", "for honor standard edition", "for honor marching fire edition",
    //    "scribblenauts", "lego batman 3", "the witcher 3" };
    var steamGameIds = new List<int>() { 304390, 304390, 304390, 304390,
                                         218680, 218680, 218680, 218680,
                                         313690, 313690, 313690, 313690,
                                         292030, 292030, 292030, 292030};

    for (int i = 0; i < steamGameIds.Count; i++)
    {
        //int gameId = 0;
        //try
        //{
        //    gameId = gameRepository.GetId(gameNames[i]);
        //}
        //catch
        //{
        //    continue;
        //}

        var entities = await searchSearcher.GetPrices(gameNames[i], steamGameIds[i]);

        organizeController.JoinByName(entities);

        //foreach (var entity in entities)
        //{
        //    Console.WriteLine();
        //    Console.WriteLine(string.Concat("Store: ", entity.History.StoreName), ".");
        //    Console.WriteLine(string.Concat("Name: ", entity.Game.Name), ".");
        //    Console.WriteLine(string.Concat("Current price: R$ ", entity.GamePrices.CurrentPrice), ".");
        //    Console.WriteLine(string.Concat("Video url: ", entity.Game.Video), ".");
        //    Console.WriteLine(string.Concat("Image url: ", entity.Game.Image), ".");
        //}

        //Console.WriteLine("============================================================================");

        //foreach (var e in entities)
        //{
        //    e.Game.GameId = gameId;
        //    e.GamePrices.GameId = gameId;
        //    e.History.GameId = gameId;
        //    e.GamePrices.GameId = gameId;
        //    gameRepository.Update(e.Game);
        //    historyRepository.AddOne(e.History);


        //    var formattedGameName = e.Game.Name.Replace(":", string.Empty).Replace("-", string.Empty);

        //    if (formattedGameName.ToLower().Equals(gameNames[i], StringComparison.CurrentCultureIgnoreCase))
        //    {
        //        var dbGamePrices = gamePricesRepository.FindByGameId(gameId);

        //        if (dbGamePrices == null)
        //            gamePricesRepository.AddOne(e.GamePrices);
        //        else
        //        {
        //            if (dbGamePrices.CurrentPrice != e.GamePrices.CurrentPrice)
        //                gamePricesRepository.Update(e.GamePrices);
        //        }
        //    }
        //}
    }

    Console.WriteLine("Search end...");
});

app.MapGet("/gameInfo/{id}", async (
    int id,
    [FromServices] IRepository<Genre> genreRepository,
    [FromServices] IRepository<History> historyRepository,
    [FromServices] IRepository<Game> gameRepository,
    [FromServices] IRepository<GamePrices> gamePricesRepository,
    [FromServices] SearchController priceSearcher) =>
{
    var game = gameRepository.FindByGameId(id);
    var gamePrice = gamePricesRepository.FindByGameId(id);
    var history = historyRepository.FindByGameId(id);

    
});

app.MapGet("/all/", async (
    [FromServices] IRepository<Game> gameRepository) =>
{
    var games = gameRepository.FindAll();
    return games;
});

app.Run();


