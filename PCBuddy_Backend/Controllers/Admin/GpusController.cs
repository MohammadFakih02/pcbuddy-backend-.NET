using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCBuddy_Backend.Data;
using PCBuddy_Backend.Models;
using PCBuddy_Backend.Utils;

namespace PCBuddy_Backend.Controllers.Admin
{
    [Authorize(AuthenticationSchemes = "Cookies", Roles = "ADMIN")]
    [Route("Admin/Gpus")]
    public class GpusController : Controller
    {
        private readonly AppDbContext _context;

        public GpusController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index(int? pageNumber)
        {
            int pageSize = 100;
            var gpus = _context.Gpus.Where(g => !g.IsDeleted).OrderBy(g => g.Name);
            return View(await PaginatedList<Gpu>.CreateAsync(gpus.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Admin/Gpus/Details/5
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var gpu = await _context.Gpus.FirstOrDefaultAsync(m => m.Id == id);
            if (gpu == null) return NotFound();

            return View(gpu);
        }

        // GET: Admin/Gpus/Create
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Gpus/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Gpu gpu)
        {
            if (ModelState.IsValid)
            {
                gpu.UpdatedAt = DateTime.UtcNow;
                gpu.IsDeleted = false;
                _context.Add(gpu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(gpu);
        }

        // GET: Admin/Gpus/Edit/5
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var gpu = await _context.Gpus.FindAsync(id);
            if (gpu == null) return NotFound();

            return View(gpu);
        }

        // POST: Admin/Gpus/Edit/5
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Gpu gpu)
        {
            if (id != gpu.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    gpu.UpdatedAt = DateTime.UtcNow;
                    _context.Update(gpu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GpuExists(gpu.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(gpu);
        }

        // GET: Admin/Gpus/Delete/5
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var gpu = await _context.Gpus.FirstOrDefaultAsync(m => m.Id == id);
            if (gpu == null) return NotFound();

            return View(gpu);
        }

        // POST: Admin/Gpus/Delete/5
        [HttpPost("Delete/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gpu = await _context.Gpus.FindAsync(id);
            if (gpu != null)
            {
                gpu.IsDeleted = true;
                gpu.UpdatedAt = DateTime.UtcNow;
                _context.Update(gpu);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool GpuExists(int id)
        {
            return _context.Gpus.Any(e => e.Id == id);
        }
    }
}