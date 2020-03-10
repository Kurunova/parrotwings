using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        { }

        public DbSet<User> Users { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(p => p.SendingTransaction)
                .WithOne(e => e.SenderUser)
                .HasForeignKey(e => e.SenderUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasMany(p => p.ReceiveTransaction)
                .WithOne(e => e.ReceiverUser)
                .HasForeignKey(e => e.ReceiverUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Transaction>();

            modelBuilder.Entity<Transaction>()
                .HasOne(p => p.SenderUser)
                .WithMany(t => t.SendingTransaction)
                .HasForeignKey(p => p.SenderUserId);

            modelBuilder.Entity<Transaction>()
                .HasOne(p => p.ReceiverUser)
                .WithMany(t => t.ReceiveTransaction)
                .HasForeignKey(p => p.ReceiverUserId);
        }
    }
}