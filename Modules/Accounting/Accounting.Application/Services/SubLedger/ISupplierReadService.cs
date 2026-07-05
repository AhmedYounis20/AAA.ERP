using Accounting.Application.Commands.Suppliers;
using Accounting.Application.Dtos.Filters;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.Responses;

namespace Accounting.Application.Services;

/// <summary>
/// Read-side contract for supplier GET endpoints (hybrid CQRS — no writes).
/// </summary>
public interface ISupplierReadService
{
    Task<ApiResponse<IEnumerable<Supplier>>> GetAll();

    Task<ApiResponse<PaginatedResult<Supplier>>> GetPaginated(
        SubLeadgerFilterDto filter,
        CancellationToken cancellationToken = default);

    Task<ApiResponse<Supplier>> GetById(Guid id);

    Task<ApiResponse<SupplierCreateCommand>> GetNextSubLedgers(Guid? parentId);

    Task<ApiResponse<IEnumerable<Supplier>>> GetLevel(int level = 0);

    Task<ApiResponse<IEnumerable<Supplier>>> GetChildren(Guid parentId, int level = 0);
}
