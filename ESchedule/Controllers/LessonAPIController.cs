using ESchedule.Data;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ESchedule.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonAPIController : ControllerBase
    {
        private readonly EScheduleDbContext eScheduleDbContext;

        public LessonAPIController(EScheduleDbContext eScheduleDbContext)
        {
            this.eScheduleDbContext = eScheduleDbContext;
        }

        // GET: api/<LessonController>
        [HttpGet]
        public IEnumerable<string> GetAll()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<LessonController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<LessonController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // DELETE api/<LessonController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
