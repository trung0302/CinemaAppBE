using System.ComponentModel.DataAnnotations;

namespace CinemaAppBE.DTO.Theater
{
    public class AddTheaterDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public string ImageUrl { get; set; }
    }
}
