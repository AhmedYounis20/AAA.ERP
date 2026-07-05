using Accounting.Application.Commands.FinancialPeriods;
using ERP.Application.Services.BaseServices;
using ERP.Domain.Models.Dtos.FinancialPeriods;
using ERP.Domain.Models.Entities.Account.FinancialPeriods;
using Shared.Responses;

namespace Accounting.Application.Services;

public interface IFinancialPeriodService : IBaseService<FinancialPeriod, FinancialPeriodCreateCommand, FinancialPeriodUpdateCommand>
{
    Task<ApiResponse<FinancialPeriod>> GetCurrentFinancailPeriod();
    Task<ApiResponse<List<FinancialPeriodDto>>> GetDtos();
    Task<ApiResponse<FinancialPeriodDto?>> GetDto(Guid id);
    Task<ApiResponse<FinancialPeriodDto>> GetNextDefaultdata();
}
