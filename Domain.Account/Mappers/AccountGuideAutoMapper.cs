using AAA.ERP.OutputDtos;
using AutoMapper;
using Domain.Account.InputModels;
using Domain.Account.Models.Entities.AccountGuide;

namespace Domain.Account.Mappers;

public class AccountGuideAutoMapper : Profile
{
    public AccountGuideAutoMapper()
    {
        CreateMap<AccountGuide, AccountGuideDto>().ReverseMap();
        CreateMap<AccountGuide, AccountGuideInputModel>().ReverseMap();
    }
}
