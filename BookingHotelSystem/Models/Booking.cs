using System.ComponentModel.DataAnnotations;

namespace BookingHotelSystem.Models
{
    public class Booking
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }
        public User? User { get; set; }

        [Required]
        public int RoomId { get; set; }
        public Room? Room { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime CheckInDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime CheckOutDate { get; set; }

        public string PaymentStatus { get; set; } = "Pending"; // Pending, Paid, Cancelled
    }
}