using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCBuddy_Backend.Data;
using PCBuddy_Backend.Models;
using PCBuddy_Backend.Utils;

namespace PCBuddy_Backend.Controllers.Admin
{
    [Authorize(AuthenticationSchemes = "Cookies", Roles = "ADMIN")]
    [Route("Admin/Storage")]
    public class StorageController : Controller
    {
        private readonly AppDbContext _context;

        public StorageController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? pageNumber)
        {
            int pageSize = 100;
            var storage = _context.Storages.Where(s => !s.IsDeleted).OrderBy(s => s.Name);
            return View(await PaginatedList<Storage>.CreateAsync(storage.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var storage = await _context.Storages.FirstOrDefaultAsync(m => m.Id == id);
            if (storage == null) return NotFound();
            return View(storage);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Storage storage)
        {
            if (ModelState.IsValid)
            {
                storage.UpdatedAt = DateTime.UtcNow;
                storage.IsDeleted = false;
                _context.Add(storage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(storage);
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var storage = await _context.Storages.FindAsync(id);
            if (storage == null) return NotFound();
            return View(storage);
        }

        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Storage storage)
        {
            if (id != storage.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    storage.UpdatedAt = DateTime.UtcNow;
                    _context.Update(storage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Storages.Any(e => e.Id == id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(storage);
        }

        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var storage = await _context.Storages.FirstOrDefaultAsync(m => m.Id == id);
            if (storage == null) return NotFound();
            return View(storage);
        }

        [HttpPost("Delete/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var storage = await _context.Storages.FindAsync(id);
            if (storage != null)
            {
                storage.IsDeleted = true;
                storage.UpdatedAt = DateTime.UtcNow;
                _context.Update(storage);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}