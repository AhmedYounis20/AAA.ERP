using Accounting.Application.Commands.FinancialPeriods;
using Accounting.Domain.Models.Entities.FinancialPeriods;
using ERP.BuildingBlocks.Application.Responses;

namespace Accounting.Application.Services;

/// <summary>
/// Write-side workflow for financial period CRUD orchestration (Category C).
/// </summary>
public interface IFinancialPeriodWorkflowService
{
    Task<ApiResponse<FinancialPeriod>> Create(FinancialPeriodCreateCommand command);

    Task<ApiResponse<FinancialPeriod>> Update(FinancialPeriodUpdateCommand command);

    Task<ApiResponse<FinancialPeriod>> Delete(Guid id);
}
