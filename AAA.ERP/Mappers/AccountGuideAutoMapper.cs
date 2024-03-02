using AAA.ERP.InputModels;
using AAA.ERP.Models.Data.AccountGuide;
using AAA.ERP.OutputDtos;
using AutoMapper;

namespace AAA.ERP.Mappers;

public class AccountGuideAutoMapper : Profile
{
    public AccountGuideAutoMapper()
    {
        CreateMap<AccountGuide, AccountGuideDto>().ReverseMap();
        CreateMap<AccountGuide, AccountGuideInputModel>().ReverseMap();
    }
}
