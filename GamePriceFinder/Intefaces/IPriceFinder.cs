using GamePriceFinder.Handlers;
using GamePriceFinder.Http;
using GamePriceFinder.Models;

namespace GamePriceFinder.Intefaces
{
    public interface IPriceFinder
    {
        public string StoreUri { get; set; }
        public HttpHandler HttpHandler { get; set; }
        public Task<List<DatabaseEntitiesHandler>> GetPrice(string gameName);
    }
}
