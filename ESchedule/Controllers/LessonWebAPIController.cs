﻿using Microsoft.AspNetCore.Mvc;
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

        // GET: api/LessonWebAPI/2012-12-31
        //[HttpGet("{date:datetime}")]
        [HttpGet("GetLessonsByDate/{date}")]
        public async Task<ActionResult<IEnumerable<LessonViewModel>>> GetLessonsByDate(string date) //Bad Date
        {
            /*var month = date.Day;*/ //because date after transfer through http, date is backwarding, f.e day is month, month is day. idk
            var ConvertedDate = DateTime.Parse(date);
            if (_context.Lessons == null)
            {
                return NotFound();
            }

            return await _context.Lessons
                .Where(m => m.DayTime.Month == ConvertedDate.Month && m.DayTime.Year == ConvertedDate.Year)
                .OrderBy(a => a.BeginTime.Hour)
                .ToListAsync();
        }

        // GET: api/LessonWebAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LessonViewModel>> GetLessonViewModel(int id)
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

            return lessonViewModel;
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
