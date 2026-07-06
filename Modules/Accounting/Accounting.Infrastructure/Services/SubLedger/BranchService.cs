using Accounting.Application.Commands.Branches;
using Accounting.Application.Repositories;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.SubLeadgers;
using Accounting.Domain.Utility;

namespace Accounting.Infrastructure.Services;

/// <summary>
/// Branch default-data helper. CRUD writes are handled by CQRS workflow.
/// </summary>
public class BranchService(IChartOfAccountRepository chartOfAccountRepository) : IBranchService
{
    public Task<ApiResponse<BranchCreateCommand>> GetNextSubLeadgers(Guid? parentId)
        => SubLeadgers.SubLedgerNextDefaultsHelper.GetNextSubLedgersAsync<Branch, BranchCreateCommand>(
            chartOfAccountRepository,
            Guid.Parse(SD.BranchChartOfAccountId),
            parentId);
}
