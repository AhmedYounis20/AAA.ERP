using AAA.ERP.Models.Entities.Currencies;
using AAA.ERP.Models.Entities.FinancialPeriods;
using AAA.ERP.Models.Entities.GLSettings;
using AAA.ERP.Responses;
using AAA.ERP.Services.BaseServices.interfaces;

namespace AAA.ERP.Services.Interfaces;

public interface IFinancialPeriodService: IBaseService<FinancialPeriod>
{

    Task<ApiResponse> GetCurrentFinancailPeriod();
}