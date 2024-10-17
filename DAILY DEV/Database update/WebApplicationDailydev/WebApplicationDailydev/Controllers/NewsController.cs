using System.Net.Http;
using System.Xml;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Polly;
using WebApplicationDailydev.Model;
using WebApplicationDailydev.Repository;


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
      
        [HttpPost("fetch-and-save")]
        public async Task<IActionResult> FetchAndSaveNews()
        {
            // Lấy danh sách LinkRSS và SourceCategoriesID từ bảng SourceCategories
            var sourceCategoriesList = await _newsRepository.GetAllLinkRSSAndSourceCategoriesAsync();

            foreach (var sourceCategory in sourceCategoriesList)
            {
                // Lấy từng LinkRSS và SourceCategoriesID
                string linkRSS = sourceCategory.LinkRSS;
                int sourceCategoriesID = sourceCategory.SourceCategoriesID;

                // Gọi repository để fetch RSS feed từ mỗi LinkRSS
                var rssFeedContent = await _newsRepository.FetchRSSFeedAsync(linkRSS);

                // Xử lý dữ liệu từ RSS feed và lưu vào database
                var newsItems = _newsRepository.ProcessRSSData(rssFeedContent, sourceCategoriesID);

                // Chỉ thêm các tin tức có GUID không trùng lặp
                await _newsRepository.AddUniqueNewsAsync(newsItems);
            }

            return Ok("RSS News fetched and saved successfully.");
        }

    }
}
