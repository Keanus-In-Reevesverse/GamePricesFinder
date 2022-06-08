using GamePriceFinder.Responses;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Web;

namespace GamePriceFinder.Http
{
    /// <summary>
    /// Unifies all the http requests.
    /// </summary>
    public class HttpHandler
    {
        private const string SteamUri = "http://store.steampowered.com/api/";

        private const string EpicUri = "https://graphql.epicgames.com/graphql";

        private const string NuuvemUri = "https://www.nuuvem.com";

        private const string NuuvemSearchPath = "/catalog/search/";

        private const string PsnUri = "https://store.playstation.com/store/api/chihiro/00_09_000/";

        private const string PsnFirstSearchPath = "tumbler/br/pt/999/";

        private const string PsnSecondSearchPath = "?size=10&start=0";

        /// <summary>
        /// Use for the http request to Steam.
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, AppIds>> GetToSteam(int gameId)
        {
            var parameters = $"appdetails?appids={gameId}&cc=br&l=br";

            var httpClient = new HttpClient();

            httpClient.BaseAddress = new Uri(SteamUri);

            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = httpClient.GetAsync(parameters).Result;

            var jsonString = await response.Content.ReadAsStringAsync();

            return Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, AppIds>>(jsonString);
        }

        /// <summary>
        /// Use for the http request to Epic Games.
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        public async Task<EpicGamesStoreNET.Models.Response> PostToEpic(string gameName)
        {
            var httpClient = new HttpClient();

            var encoded = HttpUtility.UrlEncode(gameName).Replace(":", "%3A");
            var request = new EpicGamesStoreNET.Models.Request(encoded);
            var payload = JsonConvert.SerializeObject(request);

            payload = payload.ToString().Replace("US", "BR");
            payload = payload.ToString().Replace("en-US", "pt-BR");

            var method = new HttpMethod("POST");
            HttpContent body = new StringContent(payload);
            body.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var resp = await httpClient.PostAsync(EpicUri, body);
            var respString = await resp.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<EpicGamesStoreNET.Models.Response>(respString);
        }

        /// <summary>
        /// Use for the http request to Nuuvem.
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        public async Task<string> GetToNuuvem(string gameName)
        {
            var httpClient = new HttpClient();

            httpClient.BaseAddress = new Uri(NuuvemUri);

            var response = httpClient.GetAsync(string.Concat(NuuvemSearchPath, gameName)).Result;

            return response.Content.ReadAsStringAsync().Result;
        }

        /// <summary>
        /// Use for the http request to Playstation Store.
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        public async Task<Link[]> GetToPsn(string gameName)
        {
            var httpClient = new HttpClient();

            httpClient.BaseAddress = new Uri(PsnUri);

            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = httpClient.GetAsync(string.Concat(PsnFirstSearchPath, gameName, PsnSecondSearchPath)).Result;

            var json = response.Content.ReadAsStringAsync().Result;

            var deserializedPsnResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<PsnResponse>(json);

            return deserializedPsnResponse.links;
        }
    }
}
