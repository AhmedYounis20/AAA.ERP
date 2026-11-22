using Accounting.Domain.Models.Entities.ChartOfAccounts;

namespace Accounting.Application.Services;

/// <summary>
/// Validates a user-supplied chart-of-account parent (e.g. for Tax sub-ledgers).
/// </summary>
public interface ISubLedgerCoaParentValidator
{
    Task<ChartOfAccount> ValidateAndGetAsync(
        Guid parentAccountId,
        CancellationToken cancellationToken = default);
}
