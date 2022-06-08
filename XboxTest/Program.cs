using IGDB;
using System.Text.Json;
using XboxTest;

var clientId = "u9mw8d71zj380zoc8tau17f4ila1j6";
var clientSecret = "2kbvx3h88lmcgcknnvi7mlfdn4s4hx";

var httpClient = new HttpClient();

var address = "https://id.twitch.tv/oauth2/token";

var values = new Dictionary<string, string>
{
    {"client_id", clientId },
    {"client_secret", clientSecret },
    {"grant_type", "client_credentials" },
};

var content = new FormUrlEncodedContent(values);

var response = httpClient.PostAsync(address, content).Result;

var responseString = await response.Content.ReadAsStringAsync();

var token = JsonSerializer.Deserialize<Token>(responseString);

var igdb = new IGDBClient(
  // Found in Twitch Developer portal for your app
  clientId,
  clientSecret
);

var games = await igdb.QueryAsync<IGDB.Models.Game>(
    IGDBClient.Endpoints.Games, query: "fields *; where id = 4 ;");

var address2 = "https://api.igdb.com/v4/games";

var values2 = new Dictionary<string, string>
{
    {"Client-ID:", clientId },
    {"Authorization:", token.access_token },
    {"Accept:", "application/json" }
};

var httpClient2 = new HttpClient();

var content2 = new FormUrlEncodedContent(values2);

var response2 = httpClient2.PostAsync(address2, content2).Result;

var responseString2 = await response2.Content.ReadAsStringAsync();



Console.ReadKey();