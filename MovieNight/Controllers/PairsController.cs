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
    public class PairsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PairsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Pairs
        public async Task<IActionResult> Index()
        {
              return _context.MoviePair != null ? 
                          View(await _context.MoviePair.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.MoviePair'  is null.");
        }

        // GET: Pairs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.MoviePair == null)
            {
                return NotFound();
            }

            var moviePair = await _context.MoviePair
                .FirstOrDefaultAsync(m => m.Id == id);
            if (moviePair == null)
            {
                return NotFound();
            }

            return View(moviePair);
        }

        // GET: Pairs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Pairs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,winnerFound,winnerTop")] MoviePair moviePair)
        {
            if (ModelState.IsValid)
            {
                _context.Add(moviePair);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(moviePair);
        }

        // GET: Pairs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.MoviePair == null)
            {
                return NotFound();
            }

            var moviePair = await _context.MoviePair.FindAsync(id);
            if (moviePair == null)
            {
                return NotFound();
            }
            return View(moviePair);
        }

        // POST: Pairs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,winnerFound,winnerTop")] MoviePair moviePair)
        {
            if (id != moviePair.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(moviePair);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MoviePairExists(moviePair.Id))
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
            return View(moviePair);
        }

        // GET: Pairs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.MoviePair == null)
            {
                return NotFound();
            }

            var moviePair = await _context.MoviePair
                .FirstOrDefaultAsync(m => m.Id == id);
            if (moviePair == null)
            {
                return NotFound();
            }

            return View(moviePair);
        }

        // POST: Pairs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.MoviePair == null)
            {
                return Problem("Entity set 'ApplicationDbContext.MoviePair'  is null.");
            }
            var moviePair = await _context.MoviePair.FindAsync(id);
            if (moviePair != null)
            {
                _context.MoviePair.Remove(moviePair);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MoviePairExists(int id)
        {
          return (_context.MoviePair?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
