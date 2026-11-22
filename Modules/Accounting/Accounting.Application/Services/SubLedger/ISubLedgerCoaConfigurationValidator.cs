namespace Accounting.Application.Services;

/// <summary>
/// Validates that all required sub-ledger base chart-of-account mappings are configured in the database.
/// </summary>
public interface ISubLedgerCoaConfigurationValidator
{
    Task ValidateAsync(CancellationToken cancellationToken = default);
}
