//using System.Data.SqlClient;
//using System.Data.SqlTypes;
//using Microsoft.AspNetCore.Mvc;

//// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

//namespace WebApplicationDaily.dev.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class Category : ControllerBase
//    {
//        // GET: api/<Category>
//        [HttpGet]
//        public IEnumerable<string> Get()
//        {
//            return new string[] { "value1", "value2" };
//        }

//        // GET api/<Category>/5
//        [HttpGet("{id}")]
//        public string Get(int id)
//        {
//            return "value";
//        }

//        // POST api/<Category>
//        private static int Categoy_ID = 1;

//        [HttpPost]
//        public void Post([FromBody] string value)
//        {
//            var connectionstring = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=DailyDev;Integrated Security=True;Encrypt=True;";
//            var connection = new SqlConnection(connectionstring);
//            connection.Open();

//            var result = new List<Category>();
//            string CmdText = string.Format(@"insert into Categories (CategoryID, CategoryName) 
//                                             values (@CategoryID, @CategoryName) ")



//        }

//        // PUT api/<Category>/5
//        [HttpPut("{id}")]
//        public void Put(int id, [FromBody] string value)
//        {
//        }

//        // DELETE api/<Category>/5
//        [HttpDelete("{id}")]
//        public void Delete(int id)
//        {
//        }
//    }
//}
