using System.ComponentModel.DataAnnotations;

namespace CinemaAppBE.DTO.User
{
    public class UpdateUserDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
