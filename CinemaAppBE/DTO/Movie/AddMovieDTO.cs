using System.ComponentModel.DataAnnotations;

namespace CinemaAppBE.DTO.Movie
{
    public class AddMovieDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Language { get; set; }
        [Required]
        public string Duration { get; set; }
        [Required]
        public string PlayingDate { get; set; }
        [Required]
        public string PlayingTime { get; set; }
        [Required]
        public double TicketPrice { get; set; }
        [Required]
        public double Rating { get; set; }
        [Required]
        public string Genre { get; set; }
        [Required]
        public string TrailorUrl { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        [Required]
        public string FullImageUrl { get; set; }
        [Required]
        public bool Advice { get; set; } = false;
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public DateTime UpdatedAt { get; set; }
    }
}
