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
        }
    }
}
