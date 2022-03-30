using GamePriceFinder.Handlers;
using GamePriceFinder.Responses;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Web;

namespace GamePriceFinder.Http
{
    public class ApiStoresHandler
    {
        private const string steamUri = "http://store.steampowered.com/api/";

        private const string epicUri = "https://graphql.epicgames.com/graphql";

        private readonly PriceHandler _priceHandler;

        private readonly HttpClient _client;

        public ApiStoresHandler(PriceHandler priceHandler)
        {
            _client = new HttpClient();
            _priceHandler = priceHandler;
        }

        public async Task<Dictionary<string, AppIds>> GetToSteam(int gameId)
        {
            var parameters = $"appdetails?appids={gameId}&cc=br&l=br";

            _client.BaseAddress = new Uri(steamUri);

            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = _client.GetAsync(parameters).Result;

            var jsonString = await response.Content.ReadAsStringAsync();

            return Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, AppIds>>(jsonString);
        }

        public async Task<EpicGamesStoreNET.Models.Response> PostToEpic(string gameName)
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

            return JsonConvert.DeserializeObject<EpicGamesStoreNET.Models.Response>(respString);
        }

    }
}
