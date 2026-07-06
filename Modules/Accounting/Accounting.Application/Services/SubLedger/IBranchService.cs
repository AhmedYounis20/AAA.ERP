using Accounting.Application.Commands.Branches;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Application.Responses;

namespace Accounting.Application.Services;

/// <summary>
/// Branch default-data helper. CRUD writes are handled by CQRS workflow.
/// </summary>
public interface IBranchService
{
    Task<ApiResponse<BranchCreateCommand>> GetNextSubLeadgers(Guid? parentId);
}
