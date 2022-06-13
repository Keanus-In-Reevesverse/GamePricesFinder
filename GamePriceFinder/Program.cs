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

builder.Services.AddTransient<IRepository<GamePriceFinder.Models.Genre>, GenreRepository>();
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

app.MapGet("/", async (
    [FromServices] IRepository<GamePriceFinder.Models.Genre> genreRepository,
    [FromServices] IRepository<History> historyRepository,
    [FromServices] IRepository<Game> gameRepository,
    [FromServices] PriceSearcher priceSearcher) =>
{

    var gameNames = new List<string>() { "for honor", "for honor standard edition", "for honor marching fire edition",
        "scribblenauts", "lego batman 3", "the witcher 3" };
    var steamGameIds = new List<int>() { 304390, 304390, 304390, 218680, 313690, 292030 };

    for (int i = 0; i < 6; i++)
    {
        //var gameId = gameRepository.GetId(gameNames[i]);
        
        var idToDefine = i + 1;
        if (i == 5)
        {

        }
        var entities = await priceSearcher.GetPrices(gameNames[i], steamGameIds[i]);

        var dict = new Dictionary<string, int>();

        //for (int j = 0; j < entities.Count; j++)
        //{
        //    for (int k = j+1; k < entities.Count; k++)
        //    {
        //        if (entities[j].History.StoreName != entities[k].History.StoreName)
        //        {

        //        }
        //        else
        //        {
                    
        //        }
        //    }
        //}

        foreach (var entity in entities)
        {
            global::System.Console.WriteLine();
            global::System.Console.WriteLine(string.Concat("Store: ", entity.History.StoreName), ".");
            global::System.Console.WriteLine(string.Concat("Name: ", entity.Game.Name), ".");
            global::System.Console.WriteLine(string.Concat("Current price: R$ ", entity.GamePrices.CurrentPrice), ".");
            global::System.Console.WriteLine(string.Concat("Video url: ", entity.Game.Video), ".");
            global::System.Console.WriteLine(string.Concat("Image url: ", entity.Game.Image), ".");
        }
        global::System.Console.WriteLine("============================================================================");

        //foreach (var entity in entities)
        //{
        //    entity.History.GameId = gameId;
        //    historyRepository.AddOne(entity.History);
        //    gameRepository.Update(entity.Game);
        //}

    }

    Console.WriteLine("Search end...");


});


app.Run();


