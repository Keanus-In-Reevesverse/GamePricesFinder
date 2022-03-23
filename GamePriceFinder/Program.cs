using GamePriceFinder.Responses;
using System.Net.Http.Headers;


var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//app.MapGet("/", () => "Hello World!");
const int tropicoId = 57690;
const int rSixId = 1922250;
app.MapGet("/", async() =>
{
    var url = "http://store.steampowered.com/api/";
    var parameters = $"appdetails?appids={rSixId}&cc=br&l=br";

    var client = new HttpClient();
    client.BaseAddress = new Uri(url);

    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

    var response = client.GetAsync(parameters).Result;

    var jsonString = await response.Content.ReadAsStringAsync();

    var steamResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, AppIds>>(jsonString);

    if (response.IsSuccessStatusCode)
    {

    }

});

app.Run();
