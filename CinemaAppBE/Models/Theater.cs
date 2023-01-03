using System.ComponentModel.DataAnnotations;

namespace CinemaAppBE.Models
{
    public class Theater
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public string ImageUrl { get; set; }
    }
}
