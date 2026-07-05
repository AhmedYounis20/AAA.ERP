using Accounting.Application.Commands.Branches;
using Accounting.Application.Dtos.Filters;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.Responses;

namespace Accounting.Infrastructure.Services;

/// <summary>
/// Branch reads. Transitional: delegates to <see cref="IBranchService"/>.
/// </summary>
public class BranchReadService(IBranchService branchService) : IBranchReadService
{
    public Task<ApiResponse<IEnumerable<Branch>>> GetAll()
        => branchService.ReadAll();

    public Task<ApiResponse<PaginatedResult<Branch>>> GetPaginated(
        SubLeadgerFilterDto filter,
        CancellationToken cancellationToken = default)
        => branchService.ReadAllPaginated(filter, cancellationToken);

    public Task<ApiResponse<Branch>> GetById(Guid id)
        => branchService.ReadById(id);

    public Task<ApiResponse<BranchCreateCommand>> GetNextSubLedgers(Guid? parentId)
        => branchService.GetNextSubLeadgers(parentId);

    public async Task<ApiResponse<IEnumerable<Branch>>> GetLevel(int level = 0)
    {
        var items = await branchService.GetLevel(level);
        return new ApiResponse<IEnumerable<Branch>>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Result = items
        };
    }

    public async Task<ApiResponse<IEnumerable<Branch>>> GetChildren(Guid parentId, int level = 0)
    {
        var items = await branchService.GetChildren(parentId, level);
        return new ApiResponse<IEnumerable<Branch>>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Result = items
        };
    }
}
