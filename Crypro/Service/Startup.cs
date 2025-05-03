using AutoMapper;
using Crypro.Context;
using Crypro.Entities;
using Microsoft.Identity.Client;

namespace Crypro.Service
{
    public class Startup: IHostedService
    {

        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private readonly IMapper _mapper;

        public Startup(IConfiguration configuration, IServiceProvider serviceProvider, IMapper mapper)
        {
            _configuration = configuration;
            _serviceProvider = serviceProvider;
            _mapper = mapper;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var _dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            foreach ( var user in _dbContext.Users)
            {
                if(user.HasWallet)
                {
                    continue;
                }
                var wallet = new Wallet
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    Balance = 1000.0,
                };
                user.HasWallet = true;
                _dbContext.Users.Update(user);
                await _dbContext.Wallets.AddAsync(wallet, cancellationToken);
                
            }
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
