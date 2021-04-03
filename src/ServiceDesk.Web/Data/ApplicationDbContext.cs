using System;
using Microsoft.EntityFrameworkCore;

namespace ServiceDesk.Web.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            :base(options)
        {
        }

        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Ticket>()
                .Property(e => e.State)
                .HasConversion(
                    v => v.ToString(),
                    v => (TicketState)Enum.Parse(typeof(TicketState), v));
        }
    }
}
