using AAA.ERP.InputModels;
using AAA.ERP.Models.Entities.ChartOfAccount;
using AutoMapper;

namespace AAA.ERP.Mappers;

public class ChartOfAccountAutoMapper : Profile
{
    public ChartOfAccountAutoMapper()
    {
        //CreateMap<ChartOfAccount, AccountGuideDto>().ReverseMap();
        CreateMap<ChartOfAccount, ChartOfAccountInputModel>().ReverseMap();
    }
}