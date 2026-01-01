using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCBuddy_Backend.Data;
using PCBuddy_Backend.Models;
using PCBuddy_Backend.Utils;

namespace PCBuddy_Backend.Controllers.Admin
{
    [Authorize(AuthenticationSchemes = "Cookies", Roles = "ADMIN")]
    [Route("Admin/Cpus")] 
    public class CpusController : Controller
    {
        private readonly AppDbContext _context;

        public CpusController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index(int? pageNumber)
        {
            int pageSize = 100;
            var cpus = _context.Cpus.Where(c => !c.IsDeleted).OrderBy(c => c.Name);
            return View(await PaginatedList<Cpu>.CreateAsync(cpus.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Admin/Cpus/Details/5
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var cpu = await _context.Cpus.FirstOrDefaultAsync(m => m.Id == id);
            if (cpu == null) return NotFound();

            return View(cpu);
        }

        // GET: Admin/Cpus/Create
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Cpus/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cpu cpu)
        {
            if (ModelState.IsValid)
            {
                // Ensure defaults
                cpu.UpdatedAt = DateTime.UtcNow;
                cpu.IsDeleted = false;

                _context.Add(cpu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cpu);
        }

        // GET: Admin/Cpus/Edit/5
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var cpu = await _context.Cpus.FindAsync(id);
            if (cpu == null) return NotFound();

            return View(cpu);
        }

        // POST: Admin/Cpus/Edit/5
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Cpu cpu)
        {
            if (id != cpu.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Manually attach and set modified to avoid overwriting all fields if not passed
                    // But typically EF tracks this. For simplicity in MVC:
                    cpu.UpdatedAt = DateTime.UtcNow;
                    _context.Update(cpu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CpuExists(cpu.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(cpu);
        }

        // GET: Admin/Cpus/Delete/5
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var cpu = await _context.Cpus.FirstOrDefaultAsync(m => m.Id == id);
            if (cpu == null) return NotFound();

            return View(cpu);
        }

        // POST: Admin/Cpus/Delete/5
        [HttpPost("Delete/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cpu = await _context.Cpus.FindAsync(id);
            if (cpu != null)
            {
                // SOFT DELETE logic
                cpu.IsDeleted = true;
                cpu.UpdatedAt = DateTime.UtcNow;
                _context.Update(cpu);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool CpuExists(int id)
        {
            return _context.Cpus.Any(e => e.Id == id);
        }
    }
}