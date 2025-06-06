using Crypro.Service;

namespace Crypro.AddService
{
    public static class AddServices
    {
        public static void AddServicess(this IServiceCollection Services)
        {
            Services.AddEndpointsApiExplorer();
            Services.AddHttpClient();
            Services.AddHostedService<CryptoDataService>();
            Services.AddHostedService<LimitBackgroundService>();
            Services.AddScoped<IUserService, UserService>();
            Services.AddAutoMapper(typeof(AutoMapperProfile));
            Services.AddScoped<IWalletService, WalletService>();
            Services.AddScoped<ICryptoManagerService, CryptoManagerService>();
            Services.AddScoped<ICryptoTradeService, CryptoTradeService>();
            Services.AddScoped<IPortfolioService, PortfolioService>();
            Services.AddScoped<ITradeLogService, TradeLogService>();
            Services.AddScoped<IProfitService, ProfitService>();
            Services.AddScoped<ILimitdService, LimitedService>();
            Services.AddScoped<IFeeService, FeeService>();
            Services.AddScoped<IConvertService, ConvertService>();
        }
    }
}
