using Accounting.Application.Commands.ChartOfAccounts;
using ERP.Application.Services.BaseServices;
using ERP.Domain.Models.Entities.Account.ChartOfAccounts;
using Shared.Responses;

namespace Accounting.Application.Services;

public interface IChartOfAccountService : IBaseTreeSettingService<ChartOfAccount, ChartOfAccountCreateCommand, ChartOfAccountUpdateCommand>
{
    Task<string> GenerateNewCodeForChild(Guid? parentId);
    Task<ApiResponse<ChartOfAccountInputModel>> NextAccountDefaultData(Guid? parentId);
}
