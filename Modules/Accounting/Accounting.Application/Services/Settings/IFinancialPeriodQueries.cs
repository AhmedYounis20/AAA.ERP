using Accounting.Application.Dtos.Filters;
using Accounting.Application.Dtos.FinancialPeriods;
using Accounting.Domain.Models.Entities.FinancialPeriods;
using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.Responses;

namespace Accounting.Application.Services;

/// <summary>
/// Read-side contract for financial period GET endpoints (hybrid CQRS — no writes).
/// </summary>
public interface IFinancialPeriodQueries
{
    Task<ApiResponse<List<FinancialPeriodDto>>> GetAll();

    Task<ApiResponse<PaginatedResult<FinancialPeriod>>> GetPaginated(
        FinancialPeriodFilterDto filter,
        CancellationToken cancellationToken = default);

    Task<ApiResponse<FinancialPeriodDto?>> GetById(Guid id);

    Task<ApiResponse<FinancialPeriodDto>> GetNextDefaultData();

    Task<ApiResponse<FinancialPeriod>> GetCurrentFinancialPeriod();
}
