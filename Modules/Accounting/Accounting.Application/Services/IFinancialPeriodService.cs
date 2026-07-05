using Accounting.Application.Commands.FinancialPeriods;
using ERP.BuildingBlocks.Application.Services.BaseServices;
using Accounting.Domain.Models.Dtos.FinancialPeriods;
using Accounting.Domain.Models.Entities.FinancialPeriods;
using Shared.Responses;

namespace Accounting.Application.Services;

/// <summary>
/// Application service contract for financial period.
/// </summary>
public interface IFinancialPeriodService : IBaseService<FinancialPeriod, FinancialPeriodCreateCommand, FinancialPeriodUpdateCommand>
{
    Task<ApiResponse<FinancialPeriod>> GetCurrentFinancailPeriod();
    Task<ApiResponse<List<FinancialPeriodDto>>> GetDtos();
    Task<ApiResponse<FinancialPeriodDto?>> GetDto(Guid id);
    Task<ApiResponse<FinancialPeriodDto>> GetNextDefaultdata();
}
