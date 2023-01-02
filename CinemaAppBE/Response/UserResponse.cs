using CinemaAppBE.DTO;
using System.ComponentModel.DataAnnotations;

namespace CinemaAppBE.Response
{
    public class UserResponse
    {
        //public GetUser? User { get; set; }
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Role { get; set; }
        public string? Token { get; set; }
    }
}
