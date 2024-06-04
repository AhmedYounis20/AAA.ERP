using AAA.ERP.Models.Entities.ChartOfAccount;
using AAA.ERP.Responses;
using AAA.ERP.Services.BaseServices.Interfaces;

namespace AAA.ERP.Services.Interfaces;

public interface IChartOfAccountService : IBaseTreeSettingService<ChartOfAccount>
{
    public Task<string> GenerateNewCodeForChild(Guid? parentId);
    public Task<ApiResponse> NextAccountDefaultData(Guid? parentId);
}