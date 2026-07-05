using Accounting.Application.Commands.ChartOfAccounts;
using ERP.BuildingBlocks.Application.Services.BaseServices;
using Accounting.Domain.Models.Entities.ChartOfAccounts;
using Shared.Responses;

namespace Accounting.Application.Services;

public interface IChartOfAccountService : IBaseTreeSettingService<ChartOfAccount, ChartOfAccountCreateCommand, ChartOfAccountUpdateCommand>
{
    Task<string> GenerateNewCodeForChild(Guid? parentId);
    Task<ApiResponse<ChartOfAccountInputModel>> NextAccountDefaultData(Guid? parentId);
}
