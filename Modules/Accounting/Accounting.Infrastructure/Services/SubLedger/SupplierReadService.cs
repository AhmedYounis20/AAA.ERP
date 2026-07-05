using Accounting.Application.Commands.Suppliers;
using Accounting.Application.Dtos.Filters;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.Responses;

namespace Accounting.Infrastructure.Services;

/// <summary>
/// Supplier reads. Transitional: delegates to <see cref="ISupplierService"/>.
/// </summary>
public class SupplierReadService(ISupplierService supplierService) : ISupplierReadService
{
    public Task<ApiResponse<IEnumerable<Supplier>>> GetAll()
        => supplierService.ReadAll();

    public Task<ApiResponse<PaginatedResult<Supplier>>> GetPaginated(
        SubLeadgerFilterDto filter,
        CancellationToken cancellationToken = default)
        => supplierService.ReadAllPaginated(filter, cancellationToken);

    public Task<ApiResponse<Supplier>> GetById(Guid id)
        => supplierService.ReadById(id);

    public Task<ApiResponse<SupplierCreateCommand>> GetNextSubLedgers(Guid? parentId)
        => supplierService.GetNextSubLeadgers(parentId);

    public async Task<ApiResponse<IEnumerable<Supplier>>> GetLevel(int level = 0)
    {
        var items = await supplierService.GetLevel(level);
        return new ApiResponse<IEnumerable<Supplier>>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Result = items
        };
    }

    public async Task<ApiResponse<IEnumerable<Supplier>>> GetChildren(Guid parentId, int level = 0)
    {
        var items = await supplierService.GetChildren(parentId, level);
        return new ApiResponse<IEnumerable<Supplier>>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Result = items
        };
    }
}
