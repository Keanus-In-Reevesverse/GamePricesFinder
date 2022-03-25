using GamePriceFinder.Models;
using GamePriceFinder.Responses;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Web;

namespace GamePriceFinder
{
    public class PriceSearcher
    {
        private const string steamUri = "http://store.steampowered.com/api/";
        private const int forHonorId = 304390;

        private readonly HttpClient _client;

        public PriceSearcher()
        {
            _client = new HttpClient();
        }

        private const string epicUri = "https://graphql.epicgames.com/graphql";
        public async Task<List<String>> GetPrices(string gameName)
        {
            var forHonorSteam = await GetSteamPrice();
            var forHonorEpic = await GetEpicPrice(gameName);

            return new List<string> { forHonorSteam.Name, forHonorEpic.Name };
        }

        private async Task<Game> GetEpicPrice(string gameName)
        {
            var encoded = HttpUtility.UrlEncode(gameName).Replace(":", "%3A");
            var request = new EpicGamesStoreNET.Models.Request(encoded);
            var payload = JsonConvert.SerializeObject(request);

            payload = payload.ToString().Replace("US", "BR");
            payload = payload.ToString().Replace("en-US", "pt-BR");

            var method = new HttpMethod("POST");
            HttpContent body = new StringContent(payload);
            body.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            var resp = await _client.PostAsync(epicUri, body);
            var respString = await resp.Content.ReadAsStringAsync();

            var converted = JsonConvert.DeserializeObject<EpicGamesStoreNET.Models.Response>(respString);

            //TODO: Tratamento do preço da epic.

            return new Game { Name = converted.Data.Catalog.SearchStore.Elements[0].Title };
        }

        private async Task<Game> GetSteamPrice()
        {
            var parameters = $"appdetails?appids={forHonorId}&cc=br&l=br";

            _client.BaseAddress = new Uri(steamUri);

            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = _client.GetAsync(parameters).Result;

            var jsonString = await response.Content.ReadAsStringAsync();

            var steamResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, AppIds>>(jsonString);

            var game = new Game();

            //TODO: Tratamento do preço da steam.
            for (int responseObject = 0; responseObject < steamResponse.Count; responseObject++)
            {
                game.Name = steamResponse["304390"].data.name;
                var prices = steamResponse["304390"].data.price_overview;
            }

            return game;
        }

    }
}
