﻿using System.Net.Http;
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

        ///-----------------------------------------------------------
        [HttpPost("fetch-and-save")]
        public async Task<IActionResult> FetchAndSaveNews()
        {
            // Lấy danh sách LinkRSS từ bảng SourceCategories
            var linkRSSList = await _newsRepository.GetAllLinkRSSAsync();

            foreach (var linkRSS in linkRSSList)
            {
                // Giả sử bạn có thể lấy SourceCategoriesID từ link RSS (tùy chỉnh cách bạn quản lý ID này)
                int sourceCategoriesID = 8; // Thay bằng logic thực tế

                // Lấy dữ liệu RSS
                var newsList = await _newsRepository.FetchRSSDataAsync(linkRSS, sourceCategoriesID);

                // Lưu vào bảng News
                foreach (var news in newsList)
                {
                    await _newsRepository.AddNewsAsync(news);
                }
            }

            return Ok("RSS News fetched and saved successfully.");
        }               
    }
}
