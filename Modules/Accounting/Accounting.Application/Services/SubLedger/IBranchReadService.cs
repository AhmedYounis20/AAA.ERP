using Accounting.Application.Commands.Branches;
using Accounting.Application.Dtos.Filters;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.Responses;

namespace Accounting.Application.Services;

/// <summary>
/// Read-side contract for branch GET endpoints (hybrid CQRS — no writes).
/// </summary>
public interface IBranchReadService
{
    Task<ApiResponse<IEnumerable<Branch>>> GetAll();

    Task<ApiResponse<PaginatedResult<Branch>>> GetPaginated(
        SubLeadgerFilterDto filter,
        CancellationToken cancellationToken = default);

    Task<ApiResponse<Branch>> GetById(Guid id);

    Task<ApiResponse<BranchCreateCommand>> GetNextSubLedgers(Guid? parentId);

    Task<ApiResponse<IEnumerable<Branch>>> GetLevel(int level = 0);

    Task<ApiResponse<IEnumerable<Branch>>> GetChildren(Guid parentId, int level = 0);
}
