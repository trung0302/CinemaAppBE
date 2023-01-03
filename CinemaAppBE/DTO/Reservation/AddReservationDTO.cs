using System.ComponentModel.DataAnnotations;

namespace CinemaAppBE.DTO.Reservation
{
    public class AddReservationDTO
    {
        [Required]
        public int Quantity { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string PhuongThuc { get; set; }
        [Required]
        public Guid MovieId { get; set; }
        [Required]
        public Guid UserId { get; set; }
    }
}
