using Domain.Account.Commands.ChartOfAccounts;
using Domain.Account.InputModels;
using Domain.Account.Models.Entities.ChartOfAccounts;
using ERP.Application.Services.BaseServices;
using Shared.Responses;

namespace ERP.Application.Services.Account;

public interface IChartOfAccountService : IBaseTreeSettingService<ChartOfAccount, ChartOfAccountCreateCommand, ChartOfAccountUpdateCommand>
{
    public Task<string> GenerateNewCodeForChild(Guid? parentId);
    public Task<ApiResponse<ChartOfAccountInputModel>> NextAccountDefaultData(Guid? parentId);
}