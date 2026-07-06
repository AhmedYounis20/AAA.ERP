using Accounting.Application.Commands.Suppliers;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Application.Responses;

namespace Accounting.Application.Services;

/// <summary>
/// Supplier default-data helper. CRUD writes are handled by CQRS handlers.
/// </summary>
public interface ISupplierService
{
    Task<ApiResponse<SupplierCreateCommand>> GetNextSubLeadgers(Guid? parentId);
}
