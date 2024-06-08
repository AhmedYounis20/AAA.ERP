using AAA.ERP.OutputDtos;
using AutoMapper;
using Domain.Account.InputModels;
using Domain.Account.Models.Entities.Currencies;

namespace Domain.Account.Mappers;

public class CurrencyAutoMapper : Profile
{
    public CurrencyAutoMapper()
    {
        CreateMap<Currency, CurrencyDto>().ReverseMap();
        CreateMap<Currency, CurrencyInputModel>().ReverseMap();
    }
}
