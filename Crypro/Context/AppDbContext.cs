using Crypro.Entities;
using Microsoft.EntityFrameworkCore;

namespace Crypro.Context
{
    public class AppDbContext : DbContext
    {

        public DbSet<User> Users { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Crypto> Cryptos { get; set; }
        public DbSet<CryptoPocket> CryptoPockets { get; set; }
        public DbSet<ValueLog> ValueLogs { get; set; }
        public DbSet<TradeLog> TradeLogs { get; set; }
        public DbSet<LimitedTransaction> LimitedTransactions { get; set; }
        public DbSet<LimitLog> LimitLogs { get; set; }
        public DbSet<TransactionFee> TransactionFees { get; set; }
        public DbSet<FeeLog> FeeLogs { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.Wallet)
                .WithOne(w => w.User)
                .HasForeignKey<Wallet>(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Wallet>()
                .HasMany(w => w.CryptoPockets)
                .WithOne(cp => cp.Wallet)
                .HasForeignKey(cp => cp.WalletId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Crypto>()
                .HasMany(c => c.CryptoPocket)
                .WithOne(cp => cp.Crypto)
                .HasForeignKey(cp => cp.CryptoId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
