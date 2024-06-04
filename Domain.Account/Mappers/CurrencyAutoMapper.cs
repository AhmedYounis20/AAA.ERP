using AAA.ERP.InputModels;
using AAA.ERP.Models.Entities.AccountGuide;
using AAA.ERP.Models.Entities.Currencies;
using AAA.ERP.OutputDtos;
using AutoMapper;

namespace AAA.ERP.Mappers;

public class CurrencyAutoMapper : Profile
{
    public CurrencyAutoMapper()
    {
        CreateMap<Currency, CurrencyDto>().ReverseMap();
        CreateMap<Currency, CurrencyInputModel>().ReverseMap();
    }
}
