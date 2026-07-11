using Accounting.Application.Dtos.Entries;
using Accounting.Application.Dtos.Filters;
using Accounting.Application.Services;
using Accounting.Application.Services.Entries;
using Accounting.Contracts.Dtos;
using Accounting.Domain.Models.Entities.Entries;

namespace Accounting.Infrastructure.Services.Entries;

/// <summary>
/// Payment entry reads.
/// </summary>
public class PaymentEntryQueries(IComplexEntryService complexEntryService, IEntryService entryService)
    : IPaymentEntryQueries
{
    public Task<ApiResponse<IEnumerable<ComplexEntryDto>>> GetAll()
        => complexEntryService.GetComplexEntries(EntryType.Payment);

    public Task<ApiResponse<PaginatedResult<Entry>>> GetPaginated(
        EntryFilterDto filter,
        CancellationToken cancellationToken = default)
    {
        filter.EntryType ??= (int)EntryType.Payment;
        return entryService.GetPaginated(filter, cancellationToken);
    }

    public Task<ApiResponse<ComplexEntryDto>> GetById(Guid id)
        => complexEntryService.GetComplexEntryById(id, EntryType.Payment);

    public Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime)
        => complexEntryService.GetEntryNumber(dateTime);
}
