using Accounting.Application.Commands.Branches;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Application.Responses;
using ERP.Infrastructure.Services.Queries;

namespace Accounting.Infrastructure.Services;

/// <summary>
/// Branch read queries. Transitional: delegates to <see cref="IBranchService"/>.
/// </summary>
public class BranchQueries(IBranchService branchService)
    : BaseTreeSettingQueries<Branch, BranchCreateCommand, BranchUpdateCommand>(branchService),
      IBranchQueries
{
    public Task<ApiResponse<BranchCreateCommand>> GetNextSubLedgers(Guid? parentId)
        => branchService.GetNextSubLeadgers(parentId);
}
