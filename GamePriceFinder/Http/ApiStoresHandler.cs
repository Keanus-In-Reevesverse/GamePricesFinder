﻿using GamePriceFinder.Handlers;
using GamePriceFinder.Responses;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Web;

namespace GamePriceFinder.Http
{
    public class ApiStoresHandler
    {
        private const string SteamUri = "http://store.steampowered.com/api/";

        private const string EpicUri = "https://graphql.epicgames.com/graphql";

        private const string NuuvemUri = "https://www.nuuvem.com";

        private const string NuuvemSearchPath = "/catalog/search/";

        private readonly PriceHandler _priceHandler;

        

        public ApiStoresHandler(PriceHandler priceHandler)
        {
            _priceHandler = priceHandler;
        }

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
            body.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            var resp = await httpClient.PostAsync(EpicUri, body);
            var respString = await resp.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<EpicGamesStoreNET.Models.Response>(respString);
        }

        const string BASE_URL = "https://www.nuuvem.com";

        const string SERCH_PATH = "/catalog/search/";

        public async Task<string> GetToNuuvem(string gameName)
        {
            var httpClient2 = new HttpClient();

            httpClient2.BaseAddress = new Uri(NuuvemUri);

            var response = httpClient2.GetAsync(string.Concat(NuuvemSearchPath, gameName)).Result;

            return response.Content.ReadAsStringAsync().Result;
        }

    }
}
