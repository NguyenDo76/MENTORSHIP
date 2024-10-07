using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.ServiceModel.Syndication;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using WebApplicationDaily.dev.Controllers;

public class RssRefreshService : BackgroundService
{
    private readonly ILogger<RssRefreshService> _logger;
    private static List<RSS> _rssItems = new List<RSS>();
    private static readonly object _lock = new object();

    // Define your interval (e.g., 1 hour)
    private readonly TimeSpan _refreshInterval = TimeSpan.FromMinutes(10);

    public RssRefreshService(ILogger<RssRefreshService> logger)
    {
        _logger = logger;
    }

    public static List<RSS> GetRssItems()
    {
        lock (_lock)
        {
            return _rssItems;
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await RefreshRssFeed();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while refreshing RSS feed: {ex.Message}");
            }

            // Wait for the specified interval (1 hour in this case)
            await Task.Delay(_refreshInterval, stoppingToken);
        }
    }

    private async Task RefreshRssFeed()
    {
        _logger.LogInformation("Refreshing RSS feed...");

        var rssFeedUrl = "https://vnexpress.net/rss/kinh-doanh.rss";
        var rssItems = new List<RSS>();

        using (var httpClient = new HttpClient())
        {
            var response = await httpClient.GetStringAsync(rssFeedUrl);
            using (XmlReader reader = XmlReader.Create(new System.IO.StringReader(response)))
            {
                SyndicationFeed feed = SyndicationFeed.Load(reader);

                int rssCounter = 1;
                foreach (var item in feed.Items)
                {
                    var rssItem = new RSS
                    {
                        News_ID = rssCounter++,
                        Title = item.Title.Text,
                        Description = item.Summary.Text,
                        Link = item.Links[0].Uri.ToString(),
                        Guid = item.Id,
                        PubDate = item.PublishDate.DateTime,
                        ImageURL = ScrapeImageFromArticle(item.Links[0].Uri.ToString())
                    };

                    rssItems.Add(rssItem);
                }
            }
        }

        lock (_lock)
        {
            _rssItems = rssItems;
        }

        _logger.LogInformation("RSS feed refreshed successfully.");
    }

    private string ScrapeImageFromArticle(string articleUrl)
    {
        try
        {
            var web = new HtmlAgilityPack.HtmlWeb();
            var document = web.Load(articleUrl);

            var imgNode = document.DocumentNode.SelectSingleNode("//meta[@property='og:image']");
            if (imgNode != null)
            {
                return imgNode.GetAttributeValue("content", string.Empty);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error scraping image from article: {ex.Message}");
        }

        return string.Empty;
    }
}