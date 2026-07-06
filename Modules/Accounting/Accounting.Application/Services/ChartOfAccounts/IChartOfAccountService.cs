using Accounting.Application.Commands.ChartOfAccounts;
using Accounting.Domain.Models.Entities.ChartOfAccounts;
using ERP.BuildingBlocks.Application.Responses;

namespace Accounting.Application.Services;

/// <summary>
/// Chart-of-account domain helpers. CRUD writes are handled by CQRS handlers.
/// </summary>
public interface IChartOfAccountService
{
    Task<string> GenerateNewCodeForChild(Guid? parentId);
    Task<ApiResponse<ChartOfAccountInputModel>> NextAccountDefaultData(Guid? parentId);
}
