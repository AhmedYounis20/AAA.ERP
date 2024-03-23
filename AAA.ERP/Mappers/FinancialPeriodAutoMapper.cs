using AAA.ERP.InputModels.FinancialPeriods;
using AAA.ERP.Models.Entities.FinancialPeriods;
using AAA.ERP.Models.Entities.GLSettings;
using AutoMapper;

namespace AAA.ERP.Mappers;

public class FinancialPeriodAutoMapper : Profile
{
    public FinancialPeriodAutoMapper()
    {
        CreateMap<FinancialPeriod, FinancialPeriodInputModel>().ReverseMap();
        CreateMap<FinancialPeriod, FinancialPeriodUpdateInputModel>().ReverseMap();
    }
}
