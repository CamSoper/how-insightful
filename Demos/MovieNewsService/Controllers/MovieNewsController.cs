using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MovieNewsService.Services;
using MovieNewsService.Models;
using System.Threading;

namespace MovieNewsService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieNewsController : ControllerBase
    {

        private readonly NewsService _newsService;
        private readonly string _feedUrl = @"http://www.fandango.com/rss/movie-news.rss";
        private IMemoryCache _cache;
        private RandomService _rnd;
        private List<NewsStory> _newsItems;

        public MovieNewsController(NewsService newsService, IMemoryCache memoryCache, RandomService rndService)
        {
            _newsService = newsService;
            _cache = memoryCache;
            _rnd = rndService;
        }

        // GET: api/MovieNews
        [HttpGet]
        public IEnumerable<NewsStory> Get()
        {
            //Wait anywhere between 200ms and 3000ms
            Thread.Sleep(_rnd.GetRandom(200, 3000));

            if (!_cache.TryGetValue("newsitems", out _newsItems))
            {
                _newsItems = _newsService.GetNews(_feedUrl).Result;
                var opts = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(1));
                _cache.Set("newsitems", _newsItems);
            }

            return _newsItems;

        }

    }
}
