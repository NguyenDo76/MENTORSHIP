//using System;
//using System.Collections.Generic;
//using System.Net.Http;
//using System.ServiceModel.Syndication;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Xml;
//using HtmlAgilityPack;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;
//using WebApplicationDaily.dev.Controllers;

//public class RssRefreshService : IHostedService
//{
//    private readonly ILogger<RssRefreshService> _logger;
//    private static List<News> _rssItems = new List<News>();
//    private static readonly object _lock = new object();
//    private static int _rssCounter = 1;

//    public RssRefreshService(ILogger<RssRefreshService> logger)
//    {
//        _logger = logger;
//    }

//    public List<News> GetRssItems()
//    {
//        lock (_lock)
//        {
//            return _rssItems;
//        }
//    }

//    public async Task<List<News>> FetchRssFeedAsync(string rssFeedUrl)
//    {
//        var rssItems = new List<News>();

//        try
//        {
//            using (HttpClient client = new HttpClient())
//            {
//                HttpResponseMessage response = await client.GetAsync(rssFeedUrl);

//                if (response.IsSuccessStatusCode)
//                {
//                    string rssContent = await response.Content.ReadAsStringAsync();

//                    using (XmlReader reader = XmlReader.Create(new StringReader(rssContent)))
//                    {
//                        SyndicationFeed feed = SyndicationFeed.Load(reader);

//                        foreach (var item in feed.Items)
//                        {
//                            var rssItem = new News
//                            {
//                                News_ID = _rssCounter++,
//                                Title = item.Title.Text,
//                                Description = item.Summary.Text,
//                                Link = item.Links[0].Uri.ToString(),
//                                Guid = item.Id,
//                                PubDate = item.PublishDate.DateTime,
//                                ImageURL = ScrapeImageFromArticle(item.Links[0].Uri.ToString())
//                            };

//                            rssItems.Add(rssItem);
//                        }
//                    }

//                    lock (_lock)
//                    {
//                        _rssItems = rssItems;
//                    }

//                    _logger.LogInformation("RSS feed refreshed successfully.");
//                }
//            }
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError($"Error fetching or parsing RSS feed: {ex.Message}");
//            throw;
//        }

//        return rssItems;
//    }

//    private string ScrapeImageFromArticle(string articleUrl)
//    {
//        try
//        {
//            var web = new HtmlWeb();
//            var document = web.Load(articleUrl);

//            var imgNode = document.DocumentNode.SelectSingleNode("//meta[@property='og:image']");
//            if (imgNode != null)
//            {
//                return imgNode.GetAttributeValue("content", string.Empty);
//            }
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError($"Error scraping image from article: {ex.Message}");
//        }

//        return string.Empty;
//    }

//    public Task StartAsync(CancellationToken cancellationToken)
//    {
//        _logger.LogInformation("RssRefreshService started.");
//        _ = Task.Run(async () => await ExecuteAsync(cancellationToken), cancellationToken);
//        return Task.CompletedTask;
//    }

//    public Task StopAsync(CancellationToken cancellationToken)
//    {
//        _logger.LogInformation("RssRefreshService stopped.");
//        return Task.CompletedTask;
//    }

//    private async Task ExecuteAsync(CancellationToken stoppingToken)
//    {
//        while (!stoppingToken.IsCancellationRequested)
//        {
//            try
//            {
//                await FetchRssFeedAsync("https://your-default-rss-feed-url.com");
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError($"Error occurred while refreshing RSS feed: {ex.Message}");
//            }

//            await Task.Delay(TimeUntilNextMorning(), stoppingToken);
//        }
//    }

//    private TimeSpan TimeUntilNextMorning()
//    {
//        var nextRunTime = DateTime.Today.AddDays(1).AddHours(7);
//        return nextRunTime - DateTime.Now;
//    }
//}