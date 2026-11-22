using Accounting.Application.Commands.Branches;
using Accounting.Application.Repositories;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.ChartOfAccounts;
using Accounting.Domain.Models.Entities.SubLeadgers;
using Accounting.Infrastructure.Services.SubLeadgers;
using ERP.BuildingBlocks.Persistence.Services.Queries;

namespace Accounting.Infrastructure.Services;

/// <summary>
/// Branch read queries.
/// </summary>
public class BranchQueries(
    IBranchRepository repository,
    IChartOfAccountRepository chartOfAccountRepository,
    ISubLedgerCoaParentResolver subLedgerCoaParentResolver)
    : BaseTreeSettingRepositoryQueries<Branch>(repository),
      IBranchQueries
{
    public async Task<ApiResponse<BranchCreateCommand>> GetNextSubLedgers(Guid? parentId)
    {
        var coaParent = await subLedgerCoaParentResolver.ResolveAsync(SubLeadgerType.Branch);
        return await SubLedgerNextDefaultsHelper.GetNextSubLedgersAsync<Branch, BranchCreateCommand>(
            chartOfAccountRepository,
            coaParent.Id,
            parentId);
    }
}
