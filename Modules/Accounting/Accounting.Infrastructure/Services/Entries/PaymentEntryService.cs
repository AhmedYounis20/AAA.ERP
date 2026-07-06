using Accounting.Application.Dtos.Entries;
using Accounting.Contracts.Dtos;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.Entries;

namespace Accounting.Infrastructure.Services;

/// <summary>
/// Payment entry read-side helper. CRUD writes are handled by CQRS workflow.
/// </summary>
public class PaymentEntryService(IComplexEntryService entryService) : IPaymentEntryService
{
    public Task<ApiResponse<IEnumerable<ComplexEntryDto>>> GetComplexEntries()
        => entryService.GetComplexEntries(EntryType.Payment);

    public Task<ApiResponse<ComplexEntryDto>> GetComplexEntryById(Guid id)
        => entryService.GetComplexEntryById(id, EntryType.Payment);

    public Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime)
        => entryService.GetEntryNumber(dateTime);
}
