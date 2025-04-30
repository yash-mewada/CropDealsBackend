using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CropDeals.Models;

namespace CropDeals.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<Crop> Crops { get; set; }
        public DbSet<CropListing> CropListings { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Report> Reports { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>()
                .HasOne(a => a.Address)
                .WithOne(a => a.User)
                .HasForeignKey<ApplicationUser>(a => a.AddressId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ApplicationUser>()
                .HasOne(a => a.BankAccount)
                .WithOne(b => b.User)
                .HasForeignKey<ApplicationUser>(a => a.BankAccountId);

            builder.Entity<BankAccount>()
                .HasOne(b => b.User)
                .WithOne(u => u.BankAccount)
                .HasForeignKey<BankAccount>(b => b.UserId);

            builder.Entity<Crop>()
                .HasIndex(c => c.Name)
                .IsUnique();

            builder.Entity<CropListing>()
                .HasOne(cl => cl.Farmer)
                .WithMany()
                .HasForeignKey(cl => cl.FarmerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<CropListing>()
                .HasOne(cl => cl.Crop)
                .WithMany()
                .HasForeignKey(cl => cl.CropId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Subscription>()
                .HasOne(s => s.Dealer)
                .WithMany()
                .HasForeignKey(s => s.DealerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Subscription>()
                .HasOne(s => s.Crop)
                .WithMany()
                .HasForeignKey(s => s.CropId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Transaction>()
                .HasOne(t => t.Dealer)
                .WithMany()
                .HasForeignKey(t => t.DealerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Transaction>()
                .HasOne(t => t.Listing)
                .WithMany()
                .HasForeignKey(t => t.ListingId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Review>()
                .HasOne(r => r.Dealer)
                .WithMany()
                .HasForeignKey(r => r.DealerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Review>()
                .HasOne(r => r.Farmer)
                .WithMany()
                .HasForeignKey(r => r.FarmerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Review>()
                .HasOne(r => r.Transaction)
                .WithMany()
                .HasForeignKey(r => r.TransactionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Report>()
                .HasOne(r => r.ByUser)
                .WithMany()
                .HasForeignKey(r => r.GeneratedBy)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Report>()
                .HasOne(r => r.ForUser)
                .WithMany()
                .HasForeignKey(r => r.GeneratedFor)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
