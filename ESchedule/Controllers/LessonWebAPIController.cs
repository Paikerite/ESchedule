using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ESchedule.Data;
using ESchedule.Models;

namespace ESchedule.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonWebAPIController : ControllerBase
    {
        private readonly EScheduleDbContext _context;

        public LessonWebAPIController(EScheduleDbContext context)
        {
            _context = context;
        }

        // GET: api/LessonWebAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LessonViewModel>>> GetLessons()
        {
            if (_context.Lessons == null)
            {
                return NotFound();
            }
            return await _context.Lessons
                .OrderByDescending(a => a.Created)
                .ToListAsync();
        }

        // GET: api/LessonWebAPI/2012-12-31/{abcd@gmail.com}
        [HttpGet("GetLessonsByDateAndName/{date}/{name}")]
        public async Task<ActionResult<IEnumerable<LessonViewModel>>> GetLessonsByDateAndName(string date, string name) //Bad Date
        {
            /*var month = date.Day;*/ //because date after transfer through http, date is backwarding, f.e day is month, month is day. idk
            var ConvertedDate = DateTime.Parse(date);
            if (_context.Lessons == null)
            {
                return NotFound();
            }

            return await _context.Lessons
                .Include(c=>c.Classes).ThenInclude(a=>a.UsersAccount)
                .Where(m => m.DayTime.Month == ConvertedDate.Month 
                        && m.DayTime.Year == ConvertedDate.Year
                        && m.Classes.Any(c=>c.UsersAccount.Any(u=>u.Email == name)))
                .OrderBy(a => a.BeginTime.Hour)
                .ToListAsync();
        }

        // GET: api/LessonWebAPI/abcd@gmail.com
        [HttpGet("GetLessonsByName/{name}")]
        public async Task<ActionResult<IEnumerable<LessonViewModel>>> GetLessonsByName(string name)
        {
            if (_context.Lessons == null)
            {
                return NotFound();
            }

            return await _context.Lessons
                .Include(c => c.Classes).ThenInclude(a => a.UsersAccount)
                .Where(m => m.Classes.Any(c => c.UsersAccount.Any(u => u.Email == name)))
                .OrderBy(a => a.BeginTime.Hour)
                .ToListAsync();
        }

        // GET: api/LessonWebAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LessonViewModel>> GetLessonViewModel(int id)
        {
            try
            {
                if (_context.Lessons == null)
                {
                    return NotFound();
                }
                var lessonViewModel = await _context.Lessons
                    .Include(c=>c.Classes)
                    .FirstOrDefaultAsync(l => l.Id == id);

                if (lessonViewModel == null)
                {
                    return NotFound();
                }

                return Ok(lessonViewModel);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error updating data from the database: {e}");
            }
        }

        // PUT: api/LessonWebAPI/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLessonViewModel(int id, LessonViewModel lessonViewModel)
        {
            if (id != lessonViewModel.Id)
            {
                return BadRequest();
            }

            _context.Entry(lessonViewModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                if (!LessonViewModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                            $"Error retrieving data from the database: {e}");
                }
            }

            //return NoContent();
            return Ok(lessonViewModel);
        }

        // POST: api/LessonWebAPI
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LessonViewModel>> PostLessonViewModel(LessonViewModel lessonViewModel)
        {
            try
            {
                if (_context.Lessons == null)
                {
                    return Problem("Entity set 'EScheduleDbContext.Lessons'  is null.");
                }
                _context.Lessons.Add(lessonViewModel);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetLessonViewModel", new { id = lessonViewModel.Id }, lessonViewModel);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error updating data from the database: {e}");
            }
        }

        // PATCH: api/LessonWebAPI/AddClassesToLesson
        [HttpPatch("AddClassesToLesson")]
        public async Task<ActionResult<LessonViewModel>> AddClassesToLesson(AddClassesToLessonModel addClassesToLessonModel)
        {
            if (addClassesToLessonModel == null)
            {
                return BadRequest();
            }

            try
            {
                var lesson = await _context.Lessons
                    .Include(c=>c.Classes)
                    .FirstOrDefaultAsync(u => u.Id == addClassesToLessonModel.IdLesson);

                if (lesson == null)
                {
                    return NotFound();
                }

                var classes = await _context.Classes.Where(x => addClassesToLessonModel.IdsOfClasses.Any(y => y == x.Id)).ToListAsync();

                if (classes == null)
                {
                    return NotFound();
                }

                lesson.Classes = classes;
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetLessonViewModel), new { id = lesson.Id }, lesson);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error retrieving data from the database: {e}");
            }
        }

        // DELETE: api/LessonWebAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLessonViewModel(int id)
        {
            try
            {
                if (_context.Lessons == null)
                {
                    return NotFound();
                }
                var lessonViewModel = await _context.Lessons.FindAsync(id);
                if (lessonViewModel == null)
                {
                    return NotFound();
                }

                _context.Lessons.Remove(lessonViewModel);
                await _context.SaveChangesAsync();

                return Ok(lessonViewModel);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error updating data from the database: {e}");
            }
        }

        private bool LessonViewModelExists(int id)
        {
            return (_context.Lessons?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
