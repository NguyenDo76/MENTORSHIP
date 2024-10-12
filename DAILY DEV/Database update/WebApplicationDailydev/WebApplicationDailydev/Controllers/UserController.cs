using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebApplicationDailydev.Model;
using WebApplicationDailydev.Repository;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplicationDailydev.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _userRepository;

        public UserController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        // GET: api/<UserController>
        [HttpGet(Name = "User")]
        public ActionResult<IEnumerable<User>> GetAll()
        {
            return Ok(_userRepository.GetAll());
        }

        // GET api/<UserController>/5
        [HttpGet("{id}/result")]
        public ActionResult<User> GetId(int id)
        {
            var user = _userRepository.GetId(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // POST api/<UserController>
        [HttpPost]
        public ActionResult Add([FromBody] User user)
        {
            _userRepository.Add(user);
            return CreatedAtAction(nameof(GetId), new { id = user.UserID }, user);
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] User user)
        {
            user.UserID = id;
            if (user == null || id != user.UserID)
            {
                return BadRequest("Invalid source data.");
            }
            _userRepository.Update(user);
            return Ok(user);
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            _userRepository.Delete(id);
            return NoContent();
        }
    }
}
