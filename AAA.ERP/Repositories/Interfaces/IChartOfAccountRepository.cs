using AAA.ERP.Models.Entities.ChartOfAccount;
using AAA.ERP.Repositories.BaseRepositories.Interfaces;

namespace AAA.ERP.Repositories.Interfaces;

public interface IChartOfAccountRepository: IBaseTreeSettingRepository<ChartOfAccount> {
    public Task<string> GenerateNewCodeForChild(Guid? parentId);
    public Task<ChartOfAccount?> GetChartOfAccountByCode(string code);

}
