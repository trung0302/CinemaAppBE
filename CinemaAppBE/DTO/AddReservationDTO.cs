using System.ComponentModel.DataAnnotations;

namespace CinemaAppBE.DTO
{
    public class AddReservationDTO
    {
        public int Quantity { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public DateTime ReservationTime { get; set; }
        public Guid MovieId { get; set; }
        public Guid UserId { get; set; }
    }
}
