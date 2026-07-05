using Accounting.Application.Commands.Suppliers;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Application.Responses;
using ERP.Infrastructure.Services.Queries;

namespace Accounting.Infrastructure.Services;

/// <summary>
/// Supplier read queries. Transitional: delegates to <see cref="ISupplierService"/>.
/// </summary>
public class SupplierQueries(ISupplierService supplierService)
    : BaseTreeSettingQueries<Supplier, SupplierCreateCommand, SupplierUpdateCommand>(supplierService),
      ISupplierQueries
{
    public Task<ApiResponse<SupplierCreateCommand>> GetNextSubLedgers(Guid? parentId)
        => supplierService.GetNextSubLeadgers(parentId);
}
