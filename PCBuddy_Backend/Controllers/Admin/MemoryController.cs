using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCBuddy_Backend.Data;
using PCBuddy_Backend.Models;
using PCBuddy_Backend.Utils;

namespace PCBuddy_Backend.Controllers.Admin
{
    [Authorize(AuthenticationSchemes = "Cookies", Roles = "ADMIN")]
    [Route("Admin/Memory")]
    public class MemoryController : Controller
    {
        private readonly AppDbContext _context;

        public MemoryController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index(int? pageNumber)
        {
            int pageSize = 100;
            var memory = _context.Memory.Where(m => !m.IsDeleted).OrderBy(m => m.Name);
            return View(await PaginatedList<Memory>.CreateAsync(memory.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var memory = await _context.Memory.FirstOrDefaultAsync(m => m.Id == id);
            if (memory == null) return NotFound();
            return View(memory);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Memory memory)
        {
            if (ModelState.IsValid)
            {
                memory.UpdatedAt = DateTime.UtcNow;
                memory.IsDeleted = false;
                _context.Add(memory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(memory);
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var memory = await _context.Memory.FindAsync(id);
            if (memory == null) return NotFound();
            return View(memory);
        }

        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Memory memory)
        {
            if (id != memory.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    memory.UpdatedAt = DateTime.UtcNow;
                    _context.Update(memory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Memory.Any(e => e.Id == memory.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(memory);
        }

        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var memory = await _context.Memory.FirstOrDefaultAsync(m => m.Id == id);
            if (memory == null) return NotFound();
            return View(memory);
        }

        [HttpPost("Delete/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var memory = await _context.Memory.FindAsync(id);
            if (memory != null)
            {
                memory.IsDeleted = true;
                memory.UpdatedAt = DateTime.UtcNow;
                _context.Update(memory);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}