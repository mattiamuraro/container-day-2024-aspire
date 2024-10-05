using Cday24.Aspire.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cday24.Aspire.Data
{
    public class Cday24DbContext(DbContextOptions<Cday24DbContext> options) : DbContext(options)
    {
        public DbSet<Ticket> Tickets => Set<Ticket>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            DefineTicketValue(builder.Entity<Ticket>());
        }

        private static void DefineTicketValue(EntityTypeBuilder<Ticket> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Title).HasMaxLength(128);
            builder.Property(u => u.Description).HasMaxLength(4000);
            builder.Property(u => u.Creator).HasMaxLength(128);
        }
    }
}
