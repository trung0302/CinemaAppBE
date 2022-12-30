using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaAppBE.Models
{
    public class Token
    {
        public Guid Id { get; set; }
        public string? token { get; set; }
        public Guid UserId { get; set; }
    }
}
