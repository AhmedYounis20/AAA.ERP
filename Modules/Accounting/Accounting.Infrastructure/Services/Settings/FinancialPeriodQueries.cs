using Accounting.Application.Dtos.Filters;
using Accounting.Application.Dtos.FinancialPeriods;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.FinancialPeriods;
using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.Responses;

namespace Accounting.Infrastructure.Services;

/// <summary>
/// Financial period read queries. Transitional: delegates to <see cref="IFinancialPeriodService"/>.
/// </summary>
public class FinancialPeriodQueries(IFinancialPeriodService financialPeriodService) : IFinancialPeriodQueries
{
    public Task<ApiResponse<List<FinancialPeriodDto>>> GetAll()
        => financialPeriodService.GetDtos();

    public Task<ApiResponse<PaginatedResult<FinancialPeriod>>> GetPaginated(
        FinancialPeriodFilterDto filter,
        CancellationToken cancellationToken = default)
        => financialPeriodService.ReadAllPaginated(filter, cancellationToken);

    public Task<ApiResponse<FinancialPeriodDto?>> GetById(Guid id)
        => financialPeriodService.GetDto(id);

    public Task<ApiResponse<FinancialPeriodDto>> GetNextDefaultdata()
        => financialPeriodService.GetNextDefaultdata();

    public Task<ApiResponse<FinancialPeriod>> GetCurrentFinancialPeriod()
        => financialPeriodService.GetCurrentFinancailPeriod();
}
