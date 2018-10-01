using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesMovie.Services;
using RazorPagesMovie.ViewModels;

namespace RazorPagesMovie.Pages
{
    public class IndexModel : PageModel
    {
        private readonly NewsService _newsService;
        public List<NewsStoryViewModel> NewsItems { get; private set; }

        public IndexModel(NewsService newsService)
        {
            _newsService = newsService;
        }

        public string ErrorText { get; private set; }

        public async Task OnGet()
        {
            try
            {
                NewsItems = await _newsService.GetNews();
            }
            catch (Exception ex)
            {
                ErrorText = ex.Message;
            }
        }
    }
}
