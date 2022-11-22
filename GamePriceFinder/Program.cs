using GamePriceFinder;
using GamePriceFinder.Database;
using GamePriceFinder.Handlers;
using GamePriceFinder.MVC.Controllers;
using GamePriceFinder.MVC.Controllers.Finders;
using GamePriceFinder.MVC.Models;
using GamePriceFinder.MVC.Models.Intefaces;
using GamePriceFinder.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();


builder.Services.AddScoped<IRepository<Genre>, GenreRepository>();
builder.Services.AddScoped<IRepository<Game>, GameRepository>();
builder.Services.AddScoped<IRepository<GamePrices>, GamePricesRepository>();
builder.Services.AddScoped<IRepository<History>, HistoryRepository>();
builder.Services.AddTransient<SearchController>();
builder.Services.AddTransient<OrganizeController>();
builder.Services.AddTransient<SteamController>();
builder.Services.AddTransient<EpicController>();
builder.Services.AddTransient<NuuvemController>();
builder.Services.AddTransient<PlaystationController>();
builder.Services.AddTransient<MicrosoftController>();
builder.Services.AddTransient<DatabaseController>();

builder.Services.AddLogging(config => config.AddConsole());


builder.Services.AddScoped<DbContextOptions<DbContext>>();
var connectionString = builder.Configuration.GetSection("MySqlConnection:MySqlConnectionString").Value;
builder.Services.
    AddDbContext<DatabaseContext>(opts => opts.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

var app = builder.Build();

//app.Urls.Add("http://localhost:5000");

app.MapGet("/", async (
    [FromServices] SearchController searchSearcher,
    [FromServices] OrganizeController organizeController,
    [FromServices] DatabaseController databaseController,
    [FromServices] ILogger<Program> logger) =>
{
    logger.LogInformation("Searching for games...");
    var gameNames = new List<string>() { "for honor", "for honor starter edition", "for honor standard edition", "for honor marching fire edition",
        "scribblenauts unlimited", "scribblenauts unmasked", "scribblenauts mega pack", "scribblenauts showdown",
        "lego batman 3", "lego batman 3 além de gotham edição luxo", "lego batman 2", "lego batman",
        "the witcher 3", "the witcher 3 game of the year edition", "the witcher 3 wild hunt complete edition", "the witcher 2",
        "nioh complete edition", "thronebreaker the witcher tales" };

    var steamGameIds = new List<int>() { 304390, 304390, 304390, 304390,
                                         218680, 218680, 218680, 218680,
                                         313690, 313690, 313690, 313690,
                                         292030, 292030, 292030, 292030};

    var organizedGameLists = new List<List<EntitiesHandler>>();
    var gamesToOrganize = new List<EntitiesHandler>();
    for (int i = 0; i < steamGameIds.Count; i++)
    {
        var entities = await searchSearcher.GetPrices(gameNames[i], steamGameIds[i]);

        foreach (var g in entities.Select(a => a.Genre.Description))
        {
            //Console.WriteLine(g);
        }

        foreach (var gameEntity in entities)
        {
            gameEntity.Genre.Description = gameEntity.Genre.Description.Replace("role_playing_games", "RPG").ToUpper();
        }

        entities.ForEach(a => gamesToOrganize.Add(a));
    }

    organizedGameLists = organizeController.JoinByName(gamesToOrganize);

    try
    {
        databaseController.ManageDatabase(organizedGameLists);

        Console.WriteLine("Search end...");
    }
    catch (Exception e)
    {

    }
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


