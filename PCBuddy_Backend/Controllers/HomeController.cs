using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCBuddy_Backend.Data;
using PCBuddy_Backend.Models;

namespace PCBuddy_Backend.Controllers
{
    [Authorize(AuthenticationSchemes = "Cookies", Roles = "ADMIN")]
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Gather stats for the dashboard
            ViewBag.UserCount = await _context.Users.CountAsync();
            ViewBag.PCCount = await _context.PersonalPCs.CountAsync();
            ViewBag.GamesCount = await _context.Games.CountAsync();
            ViewBag.CpuCount = await _context.Cpus.CountAsync();

            return View();
        }
    }
}