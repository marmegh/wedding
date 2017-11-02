using Microsoft.EntityFrameworkCore;

namespace wedding.Models
{
    public class weddingContext: DbContext
    {
        public weddingContext(DbContextOptions<weddingContext> options) : base(options) {}
        public DbSet<Wedding> weddings { get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<RSVP> rsvp { get; set; }
    }
}