using Microsoft.AspNetCore.Mvc;
using WebApplicationDailydev.Model;
using WebApplicationDailydev.Repository;
using static WebApplicationDailydev.Repository.NewsRepository;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplicationDailydev.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly NewsRepository _newsRepository;
        private readonly IHttpClientFactory _httpClientFactory;

        public NewsController(NewsRepository newsRepository, IHttpClientFactory httpClientFactory)
        {
            _newsRepository = newsRepository;
            _httpClientFactory = httpClientFactory;
        }
        // GET: api/<NewsController>
        [HttpGet(Name = "News")]
        public ActionResult<IEnumerable<News>> GetAll()
        {
            return Ok(_newsRepository.GetAll());
        }

        // GET api/<NewsController>/5
        [HttpGet("{id}/result")]
        public ActionResult<News> GetId(int id)
        {
            var news = _newsRepository.GetId(id);
            if (news == null)
            {
                return NotFound();
            }
            return Ok(news);
        }

        // POST api/<NewsController>
        [HttpPost]
        public ActionResult Add([FromBody] News news)
        {
            _newsRepository.Add(news);
            return CreatedAtAction(nameof(GetId), new { id = news.News_ID }, news);
        }

        // PUT api/<NewsController>/5
        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] News news)
        {
            news.News_ID = id;
            if (news == null || id != news.News_ID)
            {
                return BadRequest("Invalid source data.");
            }
            _newsRepository.Update(news);
            return Ok(news);
        }

        // DELETE api/<NewsController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            _newsRepository.Delete(id);
            return NoContent();
        }

     

        // Method to fetch RSS feed from Category and save data Item
        [HttpGet("fetch-rss")]
        public async Task<IActionResult> FetchRssFeeds(CancellationToken cancellationToken)
        {
            try
            {
                await _newsRepository.FetchAndSaveNewsFromRSSAsync(cancellationToken);
                return Ok("RSS data fetched and saved successfully.");
               ;
            }
            catch (HttpRequestException e)
            {
                return BadRequest($"Error fetching RSS feed: {e.Message}");
            }
            catch (OperationCanceledException)
            {
                return StatusCode(408, "Request timed out.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while fetching news: " + ex.Message);
            }
        }
    }
}
