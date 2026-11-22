using Accounting.Application.Commands.Banks;
using Accounting.Application.Repositories;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.ChartOfAccounts;
using Accounting.Domain.Models.Entities.SubLeadgers;
using Accounting.Infrastructure.Services.SubLeadgers;
using ERP.BuildingBlocks.Persistence.Services.Queries;

namespace Accounting.Infrastructure.Services;

/// <summary>
/// Bank read queries.
/// </summary>
public class BankQueries(
    IBankRepository repository,
    IChartOfAccountRepository chartOfAccountRepository,
    ISubLedgerCoaParentResolver subLedgerCoaParentResolver)
    : BaseTreeSettingRepositoryQueries<Bank>(repository),
      IBankQueries
{
    public async Task<ApiResponse<BankCreateCommand>> GetNextSubLedgers(Guid? parentId)
    {
        var coaParent = await subLedgerCoaParentResolver.ResolveAsync(SubLeadgerType.Bank);
        return await SubLedgerNextDefaultsHelper.GetNextSubLedgersAsync<Bank, BankCreateCommand>(
            chartOfAccountRepository,
            coaParent.Id,
            parentId);
    }
}
