
using Crypro.Context;
using Crypro.DTO;
using Crypro.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Writers;
using System.Text.Json;

namespace Crypro.Service
{
    public class CryptoDataService : BackgroundService
    {
        public class CryptoData
        {
            public double usd { get; set; }
        }


        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<CryptoDataService> _logger;

        public CryptoDataService(IHttpClientFactory httpClientFactory, IConfiguration configuration, IServiceProvider serviceProvider, ILogger<CryptoDataService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient();
            _apiKey = configuration["ApiKey"]!;
            if (string.IsNullOrWhiteSpace(_apiKey))
            {
                _logger.LogError("API key is missing or invalid.");
                throw new Exception("API key is required.");
            }

        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("x-cg-demo-api-key", _apiKey.Trim());
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "CryptoTraderApp/1.0");
            _httpClient.DefaultRequestHeaders.Add("accept", "application/json");
            var url = "https://api.coingecko.com/api/v3/simple/price?ids=bitcoin,ethereum,cardano,solana,polkadot,chainlink,uniswap,litecoin,dogecoin,ripple,stellar,tron,algorand,near,vechain&vs_currencies=usd";


            

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                        var response = await _httpClient.GetAsync(url, stoppingToken);
                        response.EnsureSuccessStatusCode();

                        var content = await response.Content.ReadAsStringAsync();
                        var cryptoData = JsonSerializer.Deserialize<Dictionary<string, CryptoData>>(content) ?? throw new Exception("Error accured");
                        if (dbContext.Cryptos.Any())
                        {
                            if (cryptoData != null)
                            {
                                foreach (var item in cryptoData)
                                {
                                    var crypto = dbContext.Cryptos.FirstOrDefault(c => c.Name == item.Key);
                                    if (crypto == null)
                                    {
                                        continue;
                                    }
                                    crypto.value = item.Value.usd;
                                    dbContext.Cryptos.Update(crypto);
                                    var log = new ValueLog
                                    {
                                        Id = Guid.NewGuid(),
                                        CryptoId = crypto.Id,
                                        Value = crypto.value,
                                        Date = DateTime.Now
                                    };
                                    await dbContext.ValueLogs.AddAsync(log, stoppingToken);
                                }
                            }

                        }
                        else
                        {
                            foreach (var item in cryptoData)
                            {
                                var crypto = new Crypto
                                {
                                    Id = Guid.NewGuid(),
                                    Name = item.Key,
                                    value = item.Value.usd
                                };
                                var log = new ValueLog
                                {
                                    Id = Guid.NewGuid(),
                                    CryptoId = crypto.Id,
                                    Value = crypto.value,
                                    Date = DateTime.Now
                                };
                                await dbContext.Cryptos.AddAsync(crypto, stoppingToken);
                                await dbContext.ValueLogs.AddAsync(log, stoppingToken);
                                await dbContext.SaveChangesAsync(stoppingToken);
                            }

                            await DummyDataInsert();
                        }
                        await dbContext.SaveChangesAsync(stoppingToken);

                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while fetching crypto data.");
                }
                finally
                {
                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                }
            }
        }




        public async Task<bool> DummyDataInsert()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var userService = scope.ServiceProvider.GetService<IUserService>();
                var cryptoTradeService = scope.ServiceProvider.GetService<ICryptoTradeService>();
                var feeService = scope.ServiceProvider.GetService<IFeeService>();
                var cryptoList = await _context.Cryptos.ToListAsync();
                var random = new Random();

                var jsondata = JsonSerializer.Deserialize<List<UserCreateDto>>(File.ReadAllText("./TestDatas/Datas.json"));
                if (jsondata == null)
                {
                    throw new Exception("Error occurred while inserting Dummy data");
                }

                for (int i = 0; i < jsondata.Count; i++)
                {
                    User createdUser;

                    if (i == 1)
                    {
                        var user = await userService.CreateAdminAsync(jsondata[i]);
                        createdUser = await _context.Users.FindAsync(user);
                    }
                    else
                    {
                        var userId = await userService.CreateUserAsync(jsondata[i]);
                        createdUser = await _context.Users.FindAsync(userId);
                    }


                    if (createdUser == null)
                    {
                        throw new Exception("Failed to create or retrieve the user.");
                    }

                    var cryptoAmountForSold = random.NextDouble() * 32 + 1;

                    await cryptoTradeService.BuyCryptoInit(new CryptoTradeDtoToFunc
                    {
                        UserId = createdUser.Id.ToString(),
                        CryptoId = cryptoList[i].Id.ToString(),
                        Amount = ((double)(i) + cryptoAmountForSold) / 10000
                    });
                    await cryptoTradeService.BuyCryptoInit(new CryptoTradeDtoToFunc
                    {
                        UserId = createdUser.Id.ToString(),
                        CryptoId = cryptoList[i + 1].Id.ToString(),
                        Amount = ((double)(i) + (random.NextDouble() * 32 + 1)) / 10000
                    });
                    await cryptoTradeService.SellCryptoInit(new CryptoTradeDtoToFunc
                    {
                        UserId = createdUser.Id.ToString(),
                        CryptoId = cryptoList[i].Id.ToString(),
                        Amount = ((double)(i) + cryptoAmountForSold) / 11521
                    });
                }
                await feeService.AddFeeAsync(new AddFeeDto
                {
                    Amount = 2
                });

            }
            return true;
        }
    }
}