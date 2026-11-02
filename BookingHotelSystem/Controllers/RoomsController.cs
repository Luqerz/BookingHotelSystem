using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookingHotelSystem.Data;
using BookingHotelSystem.Models;

namespace BookingHotelSystem.Controllers
{
    public class RoomsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RoomsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Rooms (Boleh cari bilik guna Search Room)
        public async Task<IActionResult> Index(string searchString)
        {
            var rooms = from r in _context.Rooms
                        select r;

            if (!string.IsNullOrEmpty(searchString))
            {
                rooms = rooms.Where(s => s.RoomNumber.Contains(searchString) || s.RoomType.Contains(searchString));
            }

            return View(await rooms.ToListAsync());
        }

        // GET: Rooms/Create (Buka borang Add Room)
        public IActionResult Create()
        {
            return View();
        }

        // POST: Rooms/Create (Simpan Add Room)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RoomNumber,RoomType,Price,IsAvailable")] Room room)
        {
            if (ModelState.IsValid)
            {
                _context.Add(room);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(room);
        }

        // GET: Rooms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var room = await _context.Rooms.FindAsync(id);
            if (room == null) return NotFound();

            return View(room);
        }

        // POST: Rooms/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RoomNumber,RoomType,Price,IsAvailable")] Room room)
        {
            if (id != room.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(room);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomExists(room.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(room);
        }

        // GET: Rooms/Delete/5 (Buka halaman konfirmasi Delete Room)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var room = await _context.Rooms.FirstOrDefaultAsync(m => m.Id == id);
            if (room == null) return NotFound();

            return View(room);
        }

        // POST: Rooms/Delete/5 (Proses Delete Room)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room != null)
            {
                _context.Rooms.Remove(room);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool RoomExists(int id)
        {
            return _context.Rooms.Any(e => e.Id == id);
        }
    }
}