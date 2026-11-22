namespace Accounting.Application.Services;

/// <summary>
/// Imports accounting seed data when sub-ledger base COA configuration is missing.
/// </summary>
public interface ISubLedgerCoaSeedImporter
{
    Task ImportAccountSeedAsync(CancellationToken cancellationToken = default);
}
