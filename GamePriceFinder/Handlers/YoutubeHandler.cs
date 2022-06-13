using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;

namespace GamePriceFinder.Handlers
{
    public class YoutubeHandler
    {
        private const string YOUTUBE_URL = "https://www.youtube.com/embed/";
        private static YouTubeService _youtubeService;
        public static YouTubeService YTService
        {
            get
            {
                return GetService();
            }
            set
            {
                _youtubeService = value;
            }
        }

        private static YouTubeService GetService()
        {
            if (_youtubeService == null)
            {
                var configuration = new ConfigurationBuilder().
                SetBasePath(Directory.GetCurrentDirectory()).
                AddJsonFile("appsettings.json").
                Build();

                var apiKey = configuration.GetSection("YoutubeApiKey:ApiKey").Value;

                return new YouTubeService(new BaseClientService.Initializer()
                {
                    ApiKey = apiKey,
                });
            }

            return _youtubeService;
        }

        internal static async Task<string> GetGameTrailer(string search)
        {
            var searchRequest = YTService.Search.List("snippet");
            searchRequest.Q = search;
            searchRequest.MaxResults = 10;

            SearchListResponse searchResponse = null;

            try
            {
                searchResponse = await searchRequest.ExecuteAsync();
            }
            catch
            {
                return string.Empty;
            }

            var videos = new List<string>();

            if (searchResponse.Items.Count() <= 0)
            {
                return string.Empty;
            }

            return string.Concat(YOUTUBE_URL, searchResponse.Items[0].Id.VideoId);
        }
    }
}
