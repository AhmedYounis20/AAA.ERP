using Accounting.Application.Commands.ChartOfAccounts;
using ERP.BuildingBlocks.Application.Services.BaseServices;
using Accounting.Domain.Models.Entities.ChartOfAccounts;
using Shared.Responses;

namespace Accounting.Application.Services;

/// <summary>
/// Application service contract for chart of account.
/// </summary>
public interface IChartOfAccountService : IBaseTreeSettingService<ChartOfAccount, ChartOfAccountCreateCommand, ChartOfAccountUpdateCommand>
{
    /// <summary>Generates new code for child.</summary>
    Task<string> GenerateNewCodeForChild(Guid? parentId);
    Task<ApiResponse<ChartOfAccountInputModel>> NextAccountDefaultData(Guid? parentId);
}
