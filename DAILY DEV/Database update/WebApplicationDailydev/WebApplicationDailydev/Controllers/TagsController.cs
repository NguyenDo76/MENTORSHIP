using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebApplicationDailydev.Model;
using WebApplicationDailydev.Repository;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplicationDailydev.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly TagsRepository _tagsRepository;

        public TagsController (TagsRepository tagsRepository)
        {
            _tagsRepository = tagsRepository;
        }

        // GET: api/<TagsController>
        [HttpGet(Name = "Tags")]
        public ActionResult<IEnumerable<Tags>> GetAll()
        {
            return Ok(_tagsRepository.GetAll());
        }

        // GET api/<TagsController>/5
        [HttpGet("{id}/result")]
        public ActionResult<Tags> GetId(int id)
        {
            var tags = _tagsRepository.GetId(id);
            if (tags == null)
            {
                return NotFound();
            }
            return Ok(tags);
        }

        // POST api/<TagsController>
        [HttpPost]
        public ActionResult Add([FromBody] Tags tags)
        {
            _tagsRepository.Add(tags);
            return CreatedAtAction(nameof(GetId), new { id = tags.TagID }, tags);
        }

        // PUT api/<TagsController>/5
        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] Tags tags)
        {
            tags.TagID = id;
            if (tags == null || id != tags.TagID)
            {
                return BadRequest("Invalid source data.");
            }
            _tagsRepository.Update(tags);
            return Ok(tags);
        }

        // DELETE api/<TagsController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            _tagsRepository.Delete(id);
            return NoContent();
        }
    }
}
