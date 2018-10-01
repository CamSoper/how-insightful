using RazorPagesMovie.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace RazorPagesMovie.Services
{
    public class NewsService
    {
        private const string _newsUrl = "https://camsmovienewsservice.azurewebsites.net/api/movienews";

        public async Task<List<NewsStoryViewModel>> GetNews()
        {
            var client = new HttpClient();
            var response = await client.GetAsync(_newsUrl);
            var news = await response.Content.ReadAsAsync<List<NewsStoryViewModel>>();
            return news.OrderByDescending(story => story.Published).ToList();
        }
    }

 }
