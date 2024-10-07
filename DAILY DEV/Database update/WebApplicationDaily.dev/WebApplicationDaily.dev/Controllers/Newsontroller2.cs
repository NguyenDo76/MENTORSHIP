using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplicationDaily.dev.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Newsontroller2 : ControllerBase
    {
        // GET: api/<Newsontroller2>
        [HttpGet]
        //[HttpGet]
        //public IActionResult GetRssFeed()
        //{
        //    var rssItems = RssRefreshService.GetRssItems();
        //    return Ok(rssItems);
        //}

        // GET api/<Newsontroller2>/5
        [HttpGet]
        public IActionResult GetRssFeed()
        {
            var rssItems = RssRefreshService.GetRssItems();
            return Ok(rssItems);
        }

        //// POST api/<Newsontroller2>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<Newsontroller2>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<Newsontroller2>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
