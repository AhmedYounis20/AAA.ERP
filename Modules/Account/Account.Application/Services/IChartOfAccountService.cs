using Account.Domain.Commands;
using Account.Domain.Commands.ChartOfAccounts;
using Account.Domain.Models.Entities.ChartOfAccounts;
using Shared.Application.BaseServices;
using Shared.Domain.Responses;

namespace Account.Application.Services;

public interface IChartOfAccountService : IBaseTreeSettingService<ChartOfAccount, ChartOfAccountCreateCommand, ChartOfAccountUpdateCommand>
{
    public Task<string> GenerateNewCodeForChild(Guid? parentId);
    public Task<ApiResponse<ChartOfAccountInputModel>> NextAccountDefaultData(Guid? parentId);
}