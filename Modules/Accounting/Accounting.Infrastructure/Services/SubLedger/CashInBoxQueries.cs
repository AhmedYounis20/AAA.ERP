using Accounting.Application.Commands.CashInBoxes;
using Accounting.Application.Repositories;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.ChartOfAccounts;
using Accounting.Domain.Models.Entities.SubLeadgers;
using Accounting.Infrastructure.Services.SubLeadgers;
using ERP.BuildingBlocks.Persistence.Services.Queries;

namespace Accounting.Infrastructure.Services;

/// <summary>
/// Cash-in-box read queries.
/// </summary>
public class CashInBoxQueries(
    ICashInBoxRepository repository,
    IChartOfAccountRepository chartOfAccountRepository,
    ISubLedgerCoaParentResolver subLedgerCoaParentResolver)
    : BaseTreeSettingRepositoryQueries<CashInBox>(repository),
      ICashInBoxQueries
{
    public async Task<ApiResponse<CashInBoxCreateCommand>> GetNextSubLedgers(Guid? parentId)
    {
        var coaParent = await subLedgerCoaParentResolver.ResolveAsync(SubLeadgerType.CashInBox);
        return await SubLedgerNextDefaultsHelper.GetNextSubLedgersAsync<CashInBox, CashInBoxCreateCommand>(
            chartOfAccountRepository,
            coaParent.Id,
            parentId);
    }
}
