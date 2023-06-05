using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ESchedule.Data;
using ESchedule.Models;

namespace ESchedule.Controllers
{
    public class ClassController : Controller
    {
        private readonly EScheduleDbContext _context;

        public ClassController(EScheduleDbContext context)
        {
            _context = context;
        }

        // GET: Class
        public async Task<IActionResult> Index()
        {
              return _context.Classes != null ? 
                          View(await _context.Classes.ToListAsync()) :
                          Problem("Entity set 'EScheduleDbContext.Classes'  is null.");
        }

        // GET: Class/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Classes == null)
            {
                return NotFound();
            }

            var classViewModel = await _context.Classes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (classViewModel == null)
            {
                return NotFound();
            }

            return View(classViewModel);
        }

        // GET: Class/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Class/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,IdUserAdmin,CodeToJoin")] ClassViewModel classViewModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(classViewModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(classViewModel);
        }

        // GET: Class/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Classes == null)
            {
                return NotFound();
            }

            var classViewModel = await _context.Classes.FindAsync(id);
            if (classViewModel == null)
            {
                return NotFound();
            }
            return View(classViewModel);
        }

        // POST: Class/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,IdUserAdmin,CodeToJoin")] ClassViewModel classViewModel)
        {
            if (id != classViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(classViewModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClassViewModelExists(classViewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(classViewModel);
        }

        // GET: Class/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Classes == null)
            {
                return NotFound();
            }

            var classViewModel = await _context.Classes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (classViewModel == null)
            {
                return NotFound();
            }

            return View(classViewModel);
        }

        // POST: Class/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Classes == null)
            {
                return Problem("Entity set 'EScheduleDbContext.Classes'  is null.");
            }
            var classViewModel = await _context.Classes.FindAsync(id);
            if (classViewModel != null)
            {
                _context.Classes.Remove(classViewModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClassViewModelExists(int id)
        {
          return (_context.Classes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
