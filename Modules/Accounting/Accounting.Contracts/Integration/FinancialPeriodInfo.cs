namespace Accounting.Contracts.Integration;

public record FinancialPeriodInfo(
    Guid Id,
    DateTime StartDate,
    DateTime EndDate,
    string? YearNumber);
