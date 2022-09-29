using GamePriceFinder.MVC.Controllers;

namespace GamePriceFinder.MVC.Models.Intefaces
{
    /// <summary>
    /// Interface to get the prices, implemented by EpicFinder, MicrosoftFinder, NuuvemFinder, PlaystationStoreFinder, SteamFinder.
    /// </summary>
    public interface IPriceFinder
    {
        public string StoreUri { get; set; }
        public HttpController HttpHandler { get; set; }
    }
}
