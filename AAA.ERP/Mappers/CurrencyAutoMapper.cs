using AAA.ERP.InputModels;
using AAA.ERP.Models.Data.AccountGuide;
using AAA.ERP.Models.Data.Currencies;
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
