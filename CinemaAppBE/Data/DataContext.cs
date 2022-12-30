using CinemaAppBE.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaAppBE.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Token> Tokens { get; set; }
    }
}
