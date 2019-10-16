using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using MoviePortal.Services;
using MoviePortal.ViewModels;

namespace MoviePortal.Pages
{
    public class IndexModel : PageModel
    {
        private readonly NewsService _newsService;
        private readonly TelemetryClient _telemetry;
        private readonly string _newsServiceUrl;
        //private readonly TelemetryClient _telemetry;
        public List<NewsStoryViewModel> NewsItems { get; private set; }

        public IndexModel(NewsService newsService, TelemetryClient telemetry, IConfiguration config)
        {
            _newsService = newsService;
            this._telemetry = telemetry;
            _newsServiceUrl = config.GetValue<string>("NewsServiceUrl");
        }

        public string ErrorText { get; private set; }

        public async Task OnGet()
        {
            try
            {
                _telemetry.TrackEvent("Getting news items...");
                NewsItems = await _newsService.GetNews(_newsServiceUrl);
            }
            catch (Exception ex)
            {
                ErrorText = ex.Message;
            }
        }
    }
}
