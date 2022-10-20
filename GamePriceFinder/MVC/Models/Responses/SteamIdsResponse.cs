namespace GamePriceFinder.MVC.Models.Responses
{

    public class SteamIdsResponse
    {
        public Applist applist { get; set; }
    }

    public class Applist
    {
        public Apps apps { get; set; }
    }

    public class Apps
    {
        public App[] app { get; set; }
    }

    public class App
    {
        public int appid { get; set; }
        public string name { get; set; }
    }

}
