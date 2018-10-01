using RazorPagesMovie.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace RazorPagesMovie.Services
{
    public class NewsService
    {
        public async Task<List<NewsStoryViewModel>> GetNews(string newsUrl)
        {
            var client = new HttpClient();
            var response = await client.GetAsync(newsUrl);
            var news = await response.Content.ReadAsAsync<List<NewsStoryViewModel>>();
            return news.OrderByDescending(story => story.Published).ToList();
        }
    }

 }
