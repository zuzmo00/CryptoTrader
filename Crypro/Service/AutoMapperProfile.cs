using AutoMapper;
using Crypro.DTO;
using Crypro.Entities;

namespace Crypro.Service
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserCreateDto, User>().ReverseMap();
            CreateMap<WalletGetDto, Wallet>().ReverseMap();
            CreateMap<CryptoGetDto,Crypto>().ReverseMap();
            CreateMap<CryptoCreateDto, Crypto>().ReverseMap();
            CreateMap<CryptoTradeDtoToFunc, CryptoTradeDto>().ReverseMap();
            CreateMap<CryptoPocketDto,CryptoPocket>().ReverseMap();
            CreateMap<TransactionGetAllByUserDto, TradeLog>().ReverseMap();
            CreateMap<TradeLog, TransactinGetByTransactionId>().ReverseMap();
            CreateMap<LimitBuyDto, LimitedTransaction>().ReverseMap();
            CreateMap<LimitSellDto, LimitedTransaction>().ReverseMap();
            CreateMap<LimitBuyDto, LimitLog>().ReverseMap();
            CreateMap<LimitSellDto, LimitLog>().ReverseMap();
            CreateMap<LimitGetDto, LimitedTransaction>().ReverseMap();
            CreateMap<LimitGetDto, LimitLog>().ReverseMap();
        }
    }
}
