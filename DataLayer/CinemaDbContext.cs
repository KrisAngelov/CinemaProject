using BusinessLayer;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataLayer
{
    public class CinemaDbContext : IdentityDbContext<User>
    {
        public CinemaDbContext() : base()
        {

        }
        public CinemaDbContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=DESKTOP-IN16GJU;Database=CinemaDb;" +
                    "Trusted_Connection=True;");
            }
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().Property(c => c.UserName).IsRequired();
            modelBuilder.Entity<User>().Property(c => c.Email).IsRequired();
            modelBuilder.Entity<Ticket>().HasMany(p => p.Seats).WithOne(b => b.Ticket)
                .HasForeignKey(p => p.TicketId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Seat>().HasOne(p => p.Ticket).WithMany(b => b.Seats)
                .HasForeignKey(p => p.TicketId).OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Showtime> Showtimes { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Hall> Halls { get; set; }
    }
}