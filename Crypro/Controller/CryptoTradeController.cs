using Crypro.Service;
using Microsoft.AspNetCore.Mvc;

namespace Crypro.Controller
{
    [ApiController]
    public class CryptoTradeController : ControllerBase
    {
        private readonly ICryptoTradeService _cryptoTradeService;
        public CryptoTradeController(ICryptoTradeService cryptoTradeService)
        {
            _cryptoTradeService = cryptoTradeService;
        }
        

    }
}