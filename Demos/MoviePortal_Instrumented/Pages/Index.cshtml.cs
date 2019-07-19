using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using RazorPagesMovie.Services;
using RazorPagesMovie.ViewModels;


namespace RazorPagesMovie.Pages
{
    public class IndexModel : PageModel
    {
        private readonly NewsService _newsService;
        private readonly string _newsServiceUrl;
        private TelemetryClient _telemetryClient;
        private IMemoryCache _cache;

        public List<NewsStoryViewModel> NewsItems { get; private set; }

        public IndexModel(NewsService newsService, IConfiguration config, IMemoryCache cache)
        {
            _newsService = newsService;
            _newsServiceUrl = config.GetValue<string>("NewsServiceUrl");

            _telemetryClient = new TelemetryClient();
            _cache = cache;
        }

        public string ErrorText { get; private set; }

        public async Task OnGet()
        {
            List<NewsStoryViewModel> news;

            // Look for cache key.
            if (!_cache.TryGetValue("news", out news))
            {
                _telemetryClient.TrackEvent("Cache miss! Retrieving news items.");
                try
                {
                    news = await _newsService.GetNews(_newsServiceUrl);
                }
                catch (Exception ex)
                {
                    ErrorText = ex.Message;
                }

                // Save data in cache.
                _cache.Set("news", 
                    news, 
                    new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(1)));
            }

            NewsItems = news;
        }
    }
}
