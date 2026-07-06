using Accounting.Application.Commands.Branches;
using Accounting.Application.Repositories;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.SubLeadgers;
using Accounting.Domain.Utility;
using Accounting.Infrastructure.Services.SubLeadgers;
using ERP.Infrastructure.Services.Queries;

namespace Accounting.Infrastructure.Services;

/// <summary>
/// Branch read queries.
/// </summary>
public class BranchQueries(IBranchRepository repository, IChartOfAccountRepository chartOfAccountRepository)
    : BaseTreeSettingRepositoryQueries<Branch>(repository),
      IBranchQueries
{
    public Task<ApiResponse<BranchCreateCommand>> GetNextSubLedgers(Guid? parentId)
        => SubLedgerNextDefaultsHelper.GetNextSubLedgersAsync<Branch, BranchCreateCommand>(
            chartOfAccountRepository,
            Guid.Parse(SD.BranchChartOfAccountId),
            parentId);
}
