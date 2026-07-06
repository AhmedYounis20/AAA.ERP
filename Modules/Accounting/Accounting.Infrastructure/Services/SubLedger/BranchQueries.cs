using Accounting.Application.Commands.Branches;
using Accounting.Application.Repositories;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.Infrastructure.Services.Queries;

namespace Accounting.Infrastructure.Services;

/// <summary>
/// Branch read queries.
/// </summary>
public class BranchQueries(IBranchRepository repository, IBranchService branchService)
    : BaseTreeSettingRepositoryQueries<Branch>(repository),
      IBranchQueries
{
    public Task<ApiResponse<BranchCreateCommand>> GetNextSubLedgers(Guid? parentId)
        => branchService.GetNextSubLeadgers(parentId);
}
