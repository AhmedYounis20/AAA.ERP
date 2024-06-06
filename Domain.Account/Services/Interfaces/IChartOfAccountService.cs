using Domain.Account.Commands.ChartOfAccounts;
using Domain.Account.InputModels;
using Domain.Account.Models.Entities.ChartOfAccounts;
using Domain.Account.Services.BaseServices.interfaces;
using Shared.Responses;

namespace Domain.Account.Services.Interfaces;

public interface IChartOfAccountService : IBaseTreeSettingService<ChartOfAccount,ChartOfAccountCreateCommand,ChartOfAccountUpdateCommand>
{
    public Task<string> GenerateNewCodeForChild(Guid? parentId);
    public Task<ApiResponse<ChartOfAccountInputModel>> NextAccountDefaultData(Guid? parentId);
}