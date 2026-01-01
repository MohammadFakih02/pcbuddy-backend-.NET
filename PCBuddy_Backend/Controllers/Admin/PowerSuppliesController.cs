using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCBuddy_Backend.Data;
using PCBuddy_Backend.Models;
using PCBuddy_Backend.Utils;

namespace PCBuddy_Backend.Controllers.Admin
{
    [Authorize(AuthenticationSchemes = "Cookies", Roles = "ADMIN")]
    [Route("Admin/PowerSupplies")]
    public class PowerSuppliesController : Controller
    {
        private readonly AppDbContext _context;

        public PowerSuppliesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index(int? pageNumber)
        {
            int pageSize = 100;
            var psus = _context.PowerSupplies.Where(p => !p.IsDeleted).OrderBy(p => p.Name);
            return View(await PaginatedList<PowerSupply>.CreateAsync(psus.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var psu = await _context.PowerSupplies.FirstOrDefaultAsync(m => m.Id == id);
            if (psu == null) return NotFound();
            return View(psu);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PowerSupply psu)
        {
            if (ModelState.IsValid)
            {
                psu.UpdatedAt = DateTime.UtcNow;
                psu.IsDeleted = false;
                _context.Add(psu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(psu);
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var psu = await _context.PowerSupplies.FindAsync(id);
            if (psu == null) return NotFound();
            return View(psu);
        }

        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PowerSupply psu)
        {
            if (id != psu.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    psu.UpdatedAt = DateTime.UtcNow;
                    _context.Update(psu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.PowerSupplies.Any(e => e.Id == id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(psu);
        }

        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var psu = await _context.PowerSupplies.FirstOrDefaultAsync(m => m.Id == id);
            if (psu == null) return NotFound();
            return View(psu);
        }

        [HttpPost("Delete/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var psu = await _context.PowerSupplies.FindAsync(id);
            if (psu != null)
            {
                psu.IsDeleted = true;
                psu.UpdatedAt = DateTime.UtcNow;
                _context.Update(psu);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}