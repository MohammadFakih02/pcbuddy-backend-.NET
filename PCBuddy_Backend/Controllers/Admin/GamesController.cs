using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCBuddy_Backend.Data;
using PCBuddy_Backend.Models;

namespace PCBuddy_Backend.Controllers.Admin
{
    [Authorize(AuthenticationSchemes = "Cookies", Roles = "ADMIN")]
    [Route("Admin/Games")]
    public class GamesController : Controller
    {
        private readonly AppDbContext _context;

        public GamesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Games.Where(g => !g.IsDeleted).ToListAsync());
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var game = await _context.Games.FirstOrDefaultAsync(m => m.Id == id);
            if (game == null) return NotFound();
            return View(game);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Game game)
        {
            if (ModelState.IsValid)
            {
                game.UpdatedAt = DateTime.UtcNow;
                game.IsDeleted = false;
                _context.Add(game);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(game);
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var game = await _context.Games.FindAsync(id);
            if (game == null) return NotFound();
            return View(game);
        }

        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Game game)
        {
            if (id != game.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    game.UpdatedAt = DateTime.UtcNow;
                    _context.Update(game);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Games.Any(e => e.Id == id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(game);
        }

        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var game = await _context.Games.FirstOrDefaultAsync(m => m.Id == id);
            if (game == null) return NotFound();
            return View(game);
        }

        [HttpPost("Delete/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game != null)
            {
                game.IsDeleted = true;
                game.UpdatedAt = DateTime.UtcNow;
                _context.Update(game);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}