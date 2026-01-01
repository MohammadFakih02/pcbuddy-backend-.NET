using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCBuddy_Backend.Data;
using PCBuddy_Backend.Models;

namespace PCBuddy_Backend.Controllers.Admin
{
    [Authorize(AuthenticationSchemes = "Cookies", Roles = "ADMIN")]
    [Route("Admin/Cases")]
    public class CasesController : Controller
    {
        private readonly AppDbContext _context;

        public CasesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Cases.Where(c => !c.IsDeleted).ToListAsync());
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var pcCase = await _context.Cases.FirstOrDefaultAsync(m => m.Id == id);
            if (pcCase == null) return NotFound();
            return View(pcCase);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Case pcCase)
        {
            if (ModelState.IsValid)
            {
                pcCase.UpdatedAt = DateTime.UtcNow;
                pcCase.IsDeleted = false;
                _context.Add(pcCase);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pcCase);
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var pcCase = await _context.Cases.FindAsync(id);
            if (pcCase == null) return NotFound();
            return View(pcCase);
        }

        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Case pcCase)
        {
            if (id != pcCase.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    pcCase.UpdatedAt = DateTime.UtcNow;
                    _context.Update(pcCase);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Cases.Any(e => e.Id == id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(pcCase);
        }

        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var pcCase = await _context.Cases.FirstOrDefaultAsync(m => m.Id == id);
            if (pcCase == null) return NotFound();
            return View(pcCase);
        }

        [HttpPost("Delete/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pcCase = await _context.Cases.FindAsync(id);
            if (pcCase != null)
            {
                pcCase.IsDeleted = true;
                pcCase.UpdatedAt = DateTime.UtcNow;
                _context.Update(pcCase);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}