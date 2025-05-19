using ERP.Application.Services.BaseServices;
using ERP.Domain.Commands.Account;
using ERP.Domain.Commands.Account.ChartOfAccounts;
using ERP.Domain.Models.Entities.Account.ChartOfAccounts;
using Shared.Responses;

namespace ERP.Application.Services.Account;

public interface IChartOfAccountService : IBaseTreeSettingService<ChartOfAccount, ChartOfAccountCreateCommand, ChartOfAccountUpdateCommand>
{
    public Task<string> GenerateNewCodeForChild(Guid? parentId);
    public Task<ApiResponse<ChartOfAccountInputModel>> NextAccountDefaultData(Guid? parentId);
}