using Accounting.Application.Dtos.Filters;
using Accounting.Domain.Models.Entities.Entries;
using System.Linq.Expressions;

namespace Accounting.Infrastructure.Services.Entries;

internal static class EntryFilterSupport
{
    public static Expression<Func<Entry, bool>> BuildFilterExpression(EntryFilterDto filter)
    {
        var entryType = filter.EntryType.HasValue ? (EntryType)filter.EntryType.Value : (EntryType?)null;
        var searchTerm = string.IsNullOrWhiteSpace(filter.SearchTerm) ? null : filter.SearchTerm.Trim();
        var entryNumber = string.IsNullOrWhiteSpace(filter.EntryNumber) ? null : filter.EntryNumber.Trim();
        var documentNumber = string.IsNullOrWhiteSpace(filter.DocumentNumber) ? null : filter.DocumentNumber.Trim();

        return e =>
            (!filter.CreatedFrom.HasValue || e.CreatedAt >= filter.CreatedFrom.Value) &&
            (!filter.CreatedTo.HasValue || e.CreatedAt <= filter.CreatedTo.Value) &&
            (!entryType.HasValue || e.EntryType == entryType.Value) &&
            (!filter.BranchId.HasValue || e.BranchId == filter.BranchId.Value) &&
            (!filter.FinancialPeriodId.HasValue || e.FinancialPeriodId == filter.FinancialPeriodId.Value) &&
            (!filter.CurrencyId.HasValue || e.CurrencyId == filter.CurrencyId.Value) &&
            (!filter.EntryDateFrom.HasValue || e.EntryDate >= filter.EntryDateFrom.Value) &&
            (!filter.EntryDateTo.HasValue || e.EntryDate <= filter.EntryDateTo.Value) &&
            (entryNumber == null || (e.EntryNumber != null && e.EntryNumber.Contains(entryNumber))) &&
            (documentNumber == null || (e.DocumentNumber != null && e.DocumentNumber.Contains(documentNumber))) &&
            (searchTerm == null ||
             (e.EntryNumber != null && e.EntryNumber.Contains(searchTerm)) ||
             (e.DocumentNumber != null && e.DocumentNumber.Contains(searchTerm)) ||
             (e.ReceiverName != null && e.ReceiverName.Contains(searchTerm)) ||
             (e.Notes != null && e.Notes.Contains(searchTerm)));
    }
}
