using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebApplicationDailydev.Model;
using WebApplicationDailydev.Repository;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplicationDailydev.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SourceController : ControllerBase
    {
        private readonly SourceRepository _sourceRepository;

        public SourceController(SourceRepository sourceRepository)
        {
            _sourceRepository = sourceRepository;            
        }

        // GET: api/<SourceController>
        [HttpGet(Name = "Source")]
        public ActionResult<IEnumerable<Source>> GetAll()
        {
            return Ok(_sourceRepository.GetAll());
        }

        // GET api/<SourceController>/5
        [HttpGet("{id}/result")]
        public ActionResult<Source> GetId(int id)
        {
            var source = _sourceRepository.GetId(id);
            if (source == null)
            {
                return NotFound();
            }
            return Ok (source);
        }

        // POST api/<SourceController>
        [HttpPost]
        public ActionResult Add([FromBody] Source source)
        {
            _sourceRepository.Add(source);
            return CreatedAtAction(nameof(GetId), new { id = source.SourceID }, source);
        }

        // PUT api/<SourceController>/5
        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] Source source)
        {
            source.SourceID = id;
            if (source == null || id != source.SourceID)
            {
                return BadRequest("Invalid source data.");
            }
            _sourceRepository.Update(source);
            return Ok(source);
        }

        // DELETE api/<SourceController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            _sourceRepository.Delete(id);
            return NoContent();
        }
    }
}
