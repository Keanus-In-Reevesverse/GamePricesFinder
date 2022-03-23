using GamePriceFinder.Models;
using GamePriceFinder.Responses;
using System.Net.Http.Headers;

namespace GamePriceFinder
{
    public class PriceSearcher
    {
        private const string steamUri = "http://store.steampowered.com/api/";
        private const int forHonorId = 304390;
        public async Task GetPrices(string gameName)
        {
            await GetSteamPrice();
        }

        private async Task<Game> GetSteamPrice()
        {
            var parameters = $"appdetails?appids={forHonorId}&cc=br&l=br";

            var client = new HttpClient();
            client.BaseAddress = new Uri(steamUri);

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = client.GetAsync(parameters).Result;

            var jsonString = await response.Content.ReadAsStringAsync();

            var steamResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, AppIds>>(jsonString);

            var game = new Game();

            for (int responseObject = 0; responseObject < steamResponse.Count; responseObject++)
            {
                game.Name = steamResponse["304390"].data.name;
                var prices = steamResponse["304390"].data.price_overview;
            }

            return new Game();
        }

    }
}
