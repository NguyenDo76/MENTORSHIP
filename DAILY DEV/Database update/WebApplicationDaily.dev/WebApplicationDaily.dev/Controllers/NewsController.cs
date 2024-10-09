using System;
using System.Collections.Generic;
using System.Net.Http;
using System.ServiceModel.Syndication;
using System.Xml;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using WebApplicationDaily.dev.Controllers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplicationDaily.dev.Controllers
{
    [Route("api/rss")]
    [ApiController]
    public class RSSController : ControllerBase
    {
        //[HttpGet]
        //public IActionResult GetRssFeed()
        //{
        //    var rssItems = RssRefreshService.GetRssItems();
        //    return Ok(rssItems);
        //}
        //

        //private readonly ILogger<RSSController> _logger;
        //private readonly RssRefreshService _rssRefreshService;

        //public RSSController(ILogger<RSSController> logger, RssRefreshService rssRefreshService)
        //{
        //    _logger = logger;
        //    _rssRefreshService = rssRefreshService;
        //}

        //[HttpGet]
        //public IActionResult GetRssItems()
        //{
        //    var rssItems = _rssRefreshService.GetRssItems();
        //    return Ok(rssItems);
        //}

        //[HttpGet("fetch")]
        //public async Task<IActionResult> GetRssFeedDetail(string rssFeedUrl)
        //{
        //    var rssItems = await _rssRefreshService.FetchRssFeedAsync(rssFeedUrl);
        //    return Ok(rssItems);
        //}


        private static int _rssCounter = 1;

        [HttpGet]

        public async Task<IActionResult> GetRssFeedDetail(string rssFeedUrl)
        {
            var rssItems = new List<News>();

            try
            {
                // Fetch RSS Feed
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(rssFeedUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string rssContent = await response.Content.ReadAsStringAsync();

                        using (XmlReader reader = XmlReader.Create(new StringReader(rssContent)))
                        {
                            SyndicationFeed feed = SyndicationFeed.Load(reader);

                            foreach (var item in feed.Items)
                            {
                                var rssItem = new News
                                {
                                    News_ID = _rssCounter++,
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
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching or parsing RSS feed: {ex.Message}");
            }

            return Ok(rssItems);
        }


        private string ScrapeImageFromArticle(string articleUrl)
        {
            try
            {
                var web = new HtmlWeb();
                var document = web.Load(articleUrl);

                // You may need to modify the XPath depending on the website's structure
                var imgNode = document.DocumentNode.SelectSingleNode("//meta[@property='og:image']");

                if (imgNode != null)
                {
                    return imgNode.GetAttributeValue("content", string.Empty);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error scraping image from article: {ex.Message}");
            }

            return string.Empty; // Return empty string if no image found or an error occurs
        }
    }
}