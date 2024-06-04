using AutoMapper;
using Domain.Account.InputModels.FinancialPeriods;
using Domain.Account.Models.Entities.FinancialPeriods;

namespace Domain.Account.Mappers;

public class FinancialPeriodAutoMapper : Profile
{
    public FinancialPeriodAutoMapper()
    {
        CreateMap<FinancialPeriod, FinancialPeriodInputModel>().ReverseMap();
        CreateMap<FinancialPeriod, FinancialPeriodUpdateInputModel>().ReverseMap();
    }
}
