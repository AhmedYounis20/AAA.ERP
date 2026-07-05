using Accounting.Application.Repositories.Account.SubLeadgers;
using Accounting.Contracts.Integration;
using Microsoft.EntityFrameworkCore;

namespace Accounting.Infrastructure.Services;

public class InventoryCatalogValidationQueries : IInventoryCatalogValidationQueries
{
    private readonly ISupplierRepository _supplierRepository;

    public InventoryCatalogValidationQueries(ISupplierRepository supplierRepository)
    {
        _supplierRepository = supplierRepository;
    }

    public Task<int> CountExistingSuppliersAsync(
        IReadOnlyCollection<Guid> supplierIds,
        CancellationToken cancellationToken = default)
    {
        if (supplierIds.Count == 0)
            return Task.FromResult(0);

        return _supplierRepository.GetQuery()
            .CountAsync(e => supplierIds.Contains(e.Id), cancellationToken);
    }
}
