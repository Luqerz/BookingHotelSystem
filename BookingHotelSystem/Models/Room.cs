using System.ComponentModel.DataAnnotations;

namespace BookingHotelSystem.Models
{
    public class Room
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Room number is required")]
        public string RoomNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Room type is required")]
        public string RoomType { get; set; } = string.Empty; // Deluxe, Standard, Suite

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 10000.00, ErrorMessage = "Price must be greater than zero")]
        public decimal Price { get; set; }

        public bool IsAvailable { get; set; } = true;
    }
}