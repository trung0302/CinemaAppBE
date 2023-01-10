using System.ComponentModel.DataAnnotations;

namespace CinemaAppBE.Models
{
    public class VerifyCode
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Email { get; set; }
    }
}
