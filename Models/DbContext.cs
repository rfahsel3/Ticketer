using Microsoft.EntityFrameworkCore;

namespace Ticketer.Models {
    public class TicketerDbContext : DbContext
    {
        public TicketerDbContext(DbContextOptions<TicketerDbContext> options) : base(options) {

        }

        public DbSet<Ticket> Tickets {get; set;}
    }
}