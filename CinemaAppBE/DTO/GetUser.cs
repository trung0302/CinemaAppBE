using System.ComponentModel.DataAnnotations;

namespace CinemaAppBE.DTO
{
    public class GetUser
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Role { get; set; }
    }
}
