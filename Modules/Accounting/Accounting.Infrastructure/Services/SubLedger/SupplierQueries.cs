using Accounting.Application.Commands.Suppliers;
using Accounting.Application.Repositories;
using Accounting.Application.Repositories.Account.SubLeadgers;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.Infrastructure.Services.Queries;

namespace Accounting.Infrastructure.Services;

public class SupplierQueries(ISupplierRepository repository, ISupplierService supplierService)
    : BaseTreeSettingRepositoryQueries<Supplier>(repository),
      ISupplierQueries
{
    public Task<ApiResponse<SupplierCreateCommand>> GetNextSubLedgers(Guid? parentId)
        => supplierService.GetNextSubLeadgers(parentId);
}
