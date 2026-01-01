using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCBuddy_Backend.Data;
using PCBuddy_Backend.Models;

namespace PCBuddy_Backend.Controllers.Admin
{
    [Authorize(AuthenticationSchemes = "Cookies", Roles = "ADMIN")]
    [Route("Admin/Motherboards")]
    public class MotherboardsController : Controller
    {
        private readonly AppDbContext _context;

        public MotherboardsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Motherboards.Where(m => !m.IsDeleted).ToListAsync());
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var motherboard = await _context.Motherboards.FirstOrDefaultAsync(m => m.Id == id);
            if (motherboard == null) return NotFound();
            return View(motherboard);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Motherboard motherboard)
        {
            if (ModelState.IsValid)
            {
                motherboard.UpdatedAt = DateTime.UtcNow;
                motherboard.IsDeleted = false;
                _context.Add(motherboard);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(motherboard);
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var motherboard = await _context.Motherboards.FindAsync(id);
            if (motherboard == null) return NotFound();
            return View(motherboard);
        }

        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Motherboard motherboard)
        {
            if (id != motherboard.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    motherboard.UpdatedAt = DateTime.UtcNow;
                    _context.Update(motherboard);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Motherboards.Any(e => e.Id == id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(motherboard);
        }

        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var motherboard = await _context.Motherboards.FirstOrDefaultAsync(m => m.Id == id);
            if (motherboard == null) return NotFound();
            return View(motherboard);
        }

        [HttpPost("Delete/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var motherboard = await _context.Motherboards.FindAsync(id);
            if (motherboard != null)
            {
                motherboard.IsDeleted = true;
                motherboard.UpdatedAt = DateTime.UtcNow;
                _context.Update(motherboard);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}