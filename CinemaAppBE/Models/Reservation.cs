using System.ComponentModel.DataAnnotations;

namespace CinemaAppBE.Models
{
    public class Reservation
    {
        public Guid Id { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string PhuongThuc { get; set; }
        [Required]
        public DateTime ReservationTime { get; set; }
        public Guid MovieId { get; set; }
        public Guid UserId { get; set; }
    }
}
