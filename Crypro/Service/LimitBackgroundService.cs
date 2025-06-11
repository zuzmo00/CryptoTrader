using Crypro.Context;
using Crypro.DTO;
using Crypro.Entities;
using Microsoft.EntityFrameworkCore;

namespace Crypro.Service
{
    public class LimitBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<LimitBackgroundService> _logger;
        public LimitBackgroundService(IServiceProvider serviceProvider, ILogger<LimitBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                        var limitService = scope.ServiceProvider.GetRequiredService<ILimitdService>();
                        var limits = await dbContext.LimitedTransactions.ToListAsync(stoppingToken);
                        foreach (var limit in limits)
                        {
                            if (limits != null)
                            {
                                _logger.LogInformation($"Processing with limit{limit.Limit} order for user {limit.UserId} with crypto {limit.CryptoId} and amount {limit.Amount}.");
                                ICryptoTradeService cryptoTradeService = scope.ServiceProvider.GetRequiredService<ICryptoTradeService>();
                                var crypto = await dbContext.Cryptos.FirstOrDefaultAsync(x => x.Id == limit.CryptoId, stoppingToken) ?? throw new Exception($"Crypto not found with id: {limit.CryptoId}");
                                if (limit.Type == Enums.TradeType.Buy)
                                {
                                    if(limit.Limit >=crypto.value)
                                    {
                                        await cryptoTradeService.BuyCryptoAsync(new CryptoTradeDtoToFunc
                                        {
                                            UserId = limit.UserId.ToString(),
                                            CryptoId = limit.CryptoId.ToString(),
                                            Amount = limit.Amount
                                        });
                                        dbContext.LimitedTransactions.Remove(limit);
                                        dbContext.SaveChanges();
                                    }
                                }

                                else if (limit.Type == Enums.TradeType.Sell)
                                {
                                    if (limit.Limit < crypto.value)
                                    {
                                        await cryptoTradeService.SellCryptoAsync(new CryptoTradeDtoToFunc
                                        {
                                            UserId = limit.UserId.ToString(),
                                            CryptoId = limit.CryptoId.ToString(),
                                            Amount = limit.Amount
                                        });
                                        dbContext.LimitedTransactions.Remove(limit);
                                        dbContext.SaveChanges();
                                    }
                                }
                            }
                            else
                            {
                                _logger.LogWarning($"No limits found for user.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while executing background service.");
                }
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
