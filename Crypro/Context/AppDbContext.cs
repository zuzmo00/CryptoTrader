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

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
