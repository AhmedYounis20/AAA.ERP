namespace Accounting.Domain.Models.Entities.ChartOfAccounts;

/// <summary>
/// Identifies the posting role of a <see cref="SubLedgerBaseAccount"/> binding.
/// </summary>
public enum SubLedgerAccountRole
{
    Default,
    Asset,
    AccumulatedDepreciation,
    DepreciationExpense,
    StockUnderTransfer,
}
