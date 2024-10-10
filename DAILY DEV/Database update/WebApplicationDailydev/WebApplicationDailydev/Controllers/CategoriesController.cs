using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebApplicationDailydev.Model;
using WebApplicationDailydev.Repository;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplicationDailydev.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoriesRepository _categoriesRepository;

        public CategoriesController (CategoriesRepository categoriesRepository)
        {
            _categoriesRepository = categoriesRepository;
        }

        // GET: api/<CategoriesController>
        [HttpGet(Name = "Categories")]
        public ActionResult<IEnumerable<Categories>> GetAll()
        {
            return Ok(_categoriesRepository.GetAll());
        }

        // GET api/<CategoriesController>/5
        [HttpGet("{id}/result")]
        public ActionResult<Categories> GetId(int id)
        {
            var categories = _categoriesRepository.GetId(id);
            if (categories == null)
            {
                return NotFound();
            }
            return Ok(categories);
        }

        // POST api/<CategoriesController>
        [HttpPost]
        public ActionResult Add([FromBody] Categories categories)
        {
            _categoriesRepository.Add(categories);
            return CreatedAtAction(nameof(GetId), new { id = categories.CategoryID }, categories);
        }

        // PUT api/<CategoriesController>/5
        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] Categories categories)
        {
            categories.CategoryID = id;
            if (categories == null || id != categories.CategoryID)
            {
                return BadRequest("Invalid source data.");
            }
            _categoriesRepository.Update(categories);
            return Ok(categories);
        }

        // DELETE api/<CategoriesController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            _categoriesRepository.Delete(id);
            return NoContent();
        }
    }
}
