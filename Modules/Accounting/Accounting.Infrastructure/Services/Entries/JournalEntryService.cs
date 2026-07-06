using Accounting.Application.Dtos.Entries;
using Accounting.Contracts.Dtos;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.Entries;

namespace Accounting.Infrastructure.Services;

/// <summary>
/// Journal entry read-side helper. CRUD writes are handled by CQRS workflow.
/// </summary>
public class JournalEntryService(IEntryService entryService) : IJournalEntryService
{
    public Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime)
        => entryService.GetEntryNumber(dateTime);

    public Task<ApiResponse<IEnumerable<EntryDto>>> GetDto()
        => entryService.Get(EntryType.Journal);

    public Task<ApiResponse<EntryDto>> GetDto(Guid id)
        => entryService.Get(id, EntryType.Journal);
}
