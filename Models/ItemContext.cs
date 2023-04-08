using Microsoft.EntityFrameworkCore;

namespace hwSDI.Models
{
    public class ItemContext : DbContext
    {
        public ItemContext() { }
        public ItemContext(DbContextOptions<ItemContext> options) : base(options)
        {
            
            Database.EnsureCreated();
        }

        public virtual DbSet<Screening>? Screenings { get; set; }
        public virtual DbSet<Movie>? Movies { get; set; }
        public virtual DbSet<Guest> Guests { get; set; } = default!;
        public virtual DbSet<Ticket> Tickets { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Screening>()
                .HasOne(g => g.Movie)
                .WithMany(b => b.Screenings)
                .HasForeignKey(g => g.MovieID);

            modelBuilder.Entity<Ticket>()
                .HasKey(t => new { t.ScreeningID, t.GuestID });

            modelBuilder.Entity<Ticket>()
             .HasOne(ba => ba.Screening)
             .WithMany(b => b.Tickets)
             .HasForeignKey(ba => ba.ScreeningID);

            modelBuilder.Entity<Ticket>()
                .HasOne(ba => ba.Guest)
                .WithMany(a => a.Tickets)
                .HasForeignKey(ba => ba.GuestID);
        }
    }
}
