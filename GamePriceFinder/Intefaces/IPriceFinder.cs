using GamePriceFinder.Handlers;
using GamePriceFinder.Http;
using GamePriceFinder.Models;
using Google.Apis.YouTube.v3;

namespace GamePriceFinder.Intefaces
{
    /// <summary>
    /// Interface to get the prices, implemented by EpicFinder, MicrosoftFinder, NuuvemFinder, PlaystationStoreFinder, SteamFinder.
    /// </summary>
    public interface IPriceFinder
    {
        public string StoreUri { get; set; }
        public HttpHandler HttpHandler { get; set; }
        public Task<List<DatabaseEntitiesHandler>> GetPrice(string gameName);
    }
}
