using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookingHotelSystem.Data;
using BookingHotelSystem.Models;

namespace BookingHotelSystem.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        // GET: Users/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Users/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Id,Name,Email,Password,Role")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Login));
            }
            return View(user);
        }

        // GET: Users/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Users/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email && u.Password == password);

            if (user != null)
            {
                // Simpan maklumat sesi ringkas atau redirect ke Home/Rooms
                return RedirectToAction("Index", "Rooms");
            }

            ModelState.AddModelError("", "Invalid login attempt.");
            return View();
        }
    }
}