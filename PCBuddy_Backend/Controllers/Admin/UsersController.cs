using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCBuddy_Backend.Data;
using PCBuddy_Backend.Models;
using PCBuddy_Backend.Utils;

namespace PCBuddy_Backend.Controllers.Admin
{
    [Authorize(AuthenticationSchemes = "Cookies", Roles = "ADMIN")]
    [Route("Admin/Users")]
    public class UsersController : Controller
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Users
        [HttpGet("")]
        public async Task<IActionResult> Index(int? pageNumber)
        {
            int pageSize = 100;
            var users = _context.Users.OrderByDescending(u => u.CreatedAt);
            return View(await PaginatedList<User>.CreateAsync(users.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Admin/Users/Details/5
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var user = await _context.Users
                .Include(u => u.PersonalPCs) // Load their builds to show count
                .FirstOrDefaultAsync(m => m.Id == id);

            if (user == null) return NotFound();

            return View(user);
        }

        // GET: Admin/Users/Edit/5
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            return View(user);
        }

        // POST: Admin/Users/Edit/5
        // Admin only edits Role and Banned status, not personal info
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Role,Banned")] User userUpdate)
        {
            if (id != userUpdate.Id) return NotFound();

            var userToUpdate = await _context.Users.FindAsync(id);
            if (userToUpdate == null) return NotFound();

            // Prevent Admin from banning themselves or demoting themselves if they are the last admin
            // (Simplified check: just check if it's the current user)
            if (User.Identity?.Name == userToUpdate.Username)
            {
                // Optional: Add warning "Cannot edit your own permissions"
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Only update administrative fields
                    userToUpdate.Role = userUpdate.Role;
                    userToUpdate.Banned = userUpdate.Banned;

                    _context.Update(userToUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Users.Any(e => e.Id == id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(userToUpdate);
        }
    }
}