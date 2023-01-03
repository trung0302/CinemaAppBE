using System.ComponentModel.DataAnnotations;

namespace CinemaAppBE.DTO.User
{
    public class UserLoginDTO
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
