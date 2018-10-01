using AutoMapper;
using Microsoft.SyndicationFeed;
using Microsoft.SyndicationFeed.Rss;
using MovieNewsService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace MovieNewsService.Services
{
    public class NewsService
    {
        private readonly IMapper _mapper;

        public NewsService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<List<NewsStory>> GetNews(string feedUrl)
        {
            var news = new List<NewsStory>();
            var feedUri = new Uri(feedUrl);

            using (var xmlReader = XmlReader.Create(feedUri.ToString(),
                   new XmlReaderSettings { Async = true, DtdProcessing = DtdProcessing.Ignore }))
            {

                try
                {
                    var feedReader = new RssFeedReader(xmlReader);

                    while (await feedReader.Read())
                    {
                        switch (feedReader.ElementType)
                        {
                            // RSS Item
                            case SyndicationElementType.Item:
                                ISyndicationItem item = await feedReader.ReadItem();
                                var newsStory = _mapper.Map<NewsStory>(item);
                                newsStory.Uri = item.Links.FirstOrDefault().Uri.ToString();
                                news.Add(newsStory);
                                break;

                            // Something else
                            default:
                                break;
                        }
                    }
                }
                catch (AggregateException ae)
                {
                    throw ae.Flatten();
                }
            }

            return news.OrderByDescending(story => story.Published).ToList();
        }
    }

    public class NewsStoryProfile : Profile
    {
        public NewsStoryProfile()
        {
            // Create the AutoMapper mapping profile between the 2 objects.
            // ISyndicationItem.Id maps to NewsStory.Uri.
            CreateMap<ISyndicationItem, NewsStory>()
                .ForMember(dest => dest.Uri, opts => opts.MapFrom(src => src.Id));
        }
    }

}
