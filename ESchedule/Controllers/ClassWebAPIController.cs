using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ESchedule.Data;
using ESchedule.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace ESchedule.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassWebAPIController : ControllerBase
    {
        private readonly EScheduleDbContext _context;

        public ClassWebAPIController(EScheduleDbContext context)
        {
            _context = context;
        }

        // GET: api/ClassWebAPI/GetClassesByUserName/nor@gmail.com
        [HttpGet("GetClassesByUserName/{currectUserName}")]
        public async Task<ActionResult<IEnumerable<ClassViewModel>>> GetClassesByUserName(string currectUserName)
        {
            try
            {
                if (_context.Classes == null)
                {
                    return NotFound();
                }

                var classes = await _context.Classes
                    .Where(c => c.UsersAccount.Any(u => u.Email == currectUserName))
                    //.Include(a => a.Lessons)
                    //.Include(b => b.UsersAccount)
                    .ToListAsync();

                if (classes == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(classes);
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error retrieving data from the database: {e}");
            }
        }

        // GET: api/ClassWebAPI/GetClassByJoinCode/awgets
        [HttpGet("GetClassByJoinCode/{codeToJoin}")]
        public async Task<ActionResult<ClassViewModel>> GetClassByJoinCode(string codeToJoin)
        {
            try
            {
                if (_context.Classes == null)
                {
                    return NotFound();
                }

                var @class = await _context.Classes.FirstOrDefaultAsync(c => c.CodeToJoin == codeToJoin);

                if (@class == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(@class);
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error retrieving data from the database: {e}");
            }
        }

        // GET: api/ClassWebAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ClassViewModel>> GetClassViewModel(int id)
        {
            try
            {
                if (_context.Classes == null)
                {
                    return NotFound();
                }
                var classViewModel = await _context.Classes.FindAsync(id);

                if (classViewModel == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(classViewModel);
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error retrieving data from the database: {e}");
            }
        }

        // PUT: api/ClassWebAPI/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClassViewModel(int id, ClassViewModel classViewModel)
        {
            if (id != classViewModel.Id)
            {
                return BadRequest();
            }

            _context.Entry(classViewModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                if (!ClassViewModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        $"Error retrieving data from the database: {e}");
                }
            }

            return Ok(classViewModel);
        }

        // PATCH: api/ClassWebAPI/AddUserToClassByCode
        [HttpPatch("AddUserToClassByCode")]
        public async Task<ActionResult<ClassViewModel>> AddUserToClassByCode(JoinClassModel joinClass)
        {
            if (joinClass == null)
            {
                return BadRequest();
            }

            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == joinClass.UserName);
                if (user == null)
                {
                    return NotFound();
                }

                var @class = await _context.Classes.FirstOrDefaultAsync(c => c.CodeToJoin == joinClass.JoinCode);
                if (@class == null)
                {
                    return NotFound();
                }

                @class.UsersAccount.Add(user);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetClassViewModel), new { id = @class.Id }, @class);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error retrieving data from the database: {e}");
            }
        }

        // POST: api/ClassWebAPI
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ClassViewModel>> PostClassViewModel(ClassViewModel classViewModel)
        {
            try
            {
                if (_context.Classes == null)
                {
                    return Problem("Entity set 'EScheduleDbContext.Classes' is null.");
                }
                _context.Classes.Add(classViewModel);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetClassViewModel), new { id = classViewModel.Id }, classViewModel);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error updating data from the database: {e}");
            }

        }

        // DELETE: api/ClassWebAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClassViewModel(int id)
        {
            try
            {
                if (_context.Classes == null)
                {
                    return NotFound();
                }
                var classViewModel = await _context.Classes.FindAsync(id);
                if (classViewModel == null)
                {
                    return NotFound();
                }

                _context.Classes.Remove(classViewModel);
                await _context.SaveChangesAsync();
                return Ok(classViewModel);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error updating data from the database: {e}");
            }
        }

        private bool ClassViewModelExists(int id)
        {
            return (_context.Classes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
