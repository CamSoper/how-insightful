using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using RazorPagesMovie.Services;
using RazorPagesMovie.ViewModels;

namespace RazorPagesMovie.Pages
{
    public class IndexModel : PageModel
    {
        private readonly NewsService _newsService;
        private readonly string _newsServiceUrl;
        public List<NewsStoryViewModel> NewsItems { get; private set; }

        public IndexModel(NewsService newsService, IConfiguration config)
        {
            _newsService = newsService;
            _newsServiceUrl = config.GetValue<string>("NewsServiceUrl");
        }

        public string ErrorText { get; private set; }

        public async Task OnGet()
        {
            try
            {
                NewsItems = await _newsService.GetNews(_newsServiceUrl);
            }
            catch (Exception ex)
            {
                ErrorText = ex.Message;
            }
        }
    }
}
