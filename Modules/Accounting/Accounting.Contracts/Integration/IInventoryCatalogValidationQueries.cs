namespace Accounting.Contracts.Integration;

/// <summary>
/// Read-only validation ports for Inventory catalog writes (no Accounting domain types on the wire).
/// </summary>
public interface IInventoryCatalogValidationQueries
{
    Task<int> CountExistingSuppliersAsync(IReadOnlyCollection<Guid> supplierIds, CancellationToken cancellationToken = default);
}
