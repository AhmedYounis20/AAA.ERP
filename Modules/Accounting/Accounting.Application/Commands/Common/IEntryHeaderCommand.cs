namespace Accounting.Application.Commands.Common;

/// <summary>Shared header fields validated across journal, payment, receipt, and combined entry commands.</summary>
public interface IEntryHeaderCommand
{
    Guid BranchId { get; }
    Guid? CurrencyId { get; }
    Guid FinancialPeriodId { get; }
    string? Notes { get; }
    string? ReceiverName { get; }
    string? DocumentNumber { get; }
}
