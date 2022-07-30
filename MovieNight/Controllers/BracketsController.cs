using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieNight.Data;
using MovieNight.Models;

namespace MovieNight.Controllers
{
    public class BracketsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BracketsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Brackets
        public async Task<IActionResult> Index()
        {
              return _context.MovieBracket != null ? 
                          View(await _context.MovieBracket.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.MovieBracket'  is null.");
        }

        // GET: Brackets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.MovieBracket == null)
            {
                return NotFound();
            }

            var movieBracket = await _context.MovieBracket
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movieBracket == null)
            {
                return NotFound();
            }

            return View(movieBracket);
        }

        // GET: Brackets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Brackets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,BracketNumber")] MovieBracket movieBracket)
        {
            if (ModelState.IsValid)
            {
                _context.Add(movieBracket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(movieBracket);
        }

        // GET: Brackets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.MovieBracket == null)
            {
                return NotFound();
            }

            var movieBracket = await _context.MovieBracket.FindAsync(id);
            if (movieBracket == null)
            {
                return NotFound();
            }
            return View(movieBracket);
        }

        // POST: Brackets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,BracketNumber")] MovieBracket movieBracket)
        {
            if (id != movieBracket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movieBracket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieBracketExists(movieBracket.Id))
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
            return View(movieBracket);
        }

        // GET: Brackets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.MovieBracket == null)
            {
                return NotFound();
            }

            var movieBracket = await _context.MovieBracket
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movieBracket == null)
            {
                return NotFound();
            }

            return View(movieBracket);
        }

        // POST: Brackets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.MovieBracket == null)
            {
                return Problem("Entity set 'ApplicationDbContext.MovieBracket'  is null.");
            }
            var movieBracket = await _context.MovieBracket.FindAsync(id);
            if (movieBracket != null)
            {
                _context.MovieBracket.Remove(movieBracket);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieBracketExists(int id)
        {
          return (_context.MovieBracket?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
