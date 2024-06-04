using AutoMapper;
using Domain.Account.InputModels;
using Domain.Account.Models.Entities.ChartOfAccounts;

namespace Domain.Account.Mappers;

public class ChartOfAccountAutoMapper : Profile
{
    public ChartOfAccountAutoMapper()
    {
        //CreateMap<ChartOfAccount, AccountGuideDto>().ReverseMap();
        CreateMap<ChartOfAccount, ChartOfAccountInputModel>().ReverseMap();
    }
}