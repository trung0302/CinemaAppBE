using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaAppBE.Models
{
    public class Movie
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int SoVe { get; set; }
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
        public bool IsRelease { get; set; } = true;
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public DateTime UpdatedAt { get; set; }
        public List<Reservation> Reservations { get; set; }
    }
}
