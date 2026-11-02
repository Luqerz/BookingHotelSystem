using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookingHotelSystem.Data;
using BookingHotelSystem.Models;

namespace BookingHotelSystem.Controllers
{
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Bookings (View Booking & View Booking All User)
        public async Task<IActionResult> Index()
        {
            var bookings = _context.Bookings.Include(b => b.Room).Include(b => b.User);
            return View(await bookings.ToListAsync());
        }

        // GET: Bookings/Create (Booking Room)
        public IActionResult Create()
        {
            ViewData["RoomId"] = new SelectList(_context.Rooms.Where(r => r.IsAvailable), "Id", "RoomNumber");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name");
            return View();
        }

        // POST: Bookings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,RoomId,CheckInDate,CheckOutDate,PaymentStatus")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                booking.PaymentStatus = "Pending";
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Payment), new { id = booking.Id });
            }
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "RoomNumber", booking.RoomId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name", booking.UserId);
            return View(booking);
        }

        // GET: Bookings/Payment (Process Payment)
        public async Task<IActionResult> Payment(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings.Include(b => b.Room).Include(b => b.User).FirstOrDefaultAsync(m => m.Id == id);
            if (booking == null) return NotFound();

            return View(booking);
        }

        // POST: Bookings/ProcessPayment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessPayment(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                booking.PaymentStatus = "Paid";
                _context.Update(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Receipt), new { id = booking.Id });
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Bookings/Receipt (View Receipt)
        public async Task<IActionResult> Receipt(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings.Include(b => b.Room).Include(b => b.User).FirstOrDefaultAsync(m => m.Id == id);
            if (booking == null) return NotFound();

            return View(booking);
        }

        // GET: Bookings/Delete/5 (Cancel Booking)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings.Include(b => b.Room).Include(b => b.User).FirstOrDefaultAsync(m => m.Id == id);
            if (booking == null) return NotFound();

            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}