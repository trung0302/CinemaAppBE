using System.ComponentModel.DataAnnotations;

namespace CinemaAppBE.Models
{
    public class User
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Role { get; set; }
        public List<Reservation>? Reservations { get; set; }
        public string? VerifyToken { get; set; }
        public List<Token>? Token { get; set; }
        public List<VerifyCode>? VerifyCode { get; set; }
    }
}
