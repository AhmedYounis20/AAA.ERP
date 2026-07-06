using Accounting.Application.Dtos.Entries;
using Accounting.Contracts.Dtos;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.Entries;

namespace Accounting.Infrastructure.Services;

/// <summary>
/// Combined entry read-side helper. CRUD writes are handled by CQRS workflow.
/// </summary>
public class CombinedEntryService(IComplexEntryService entryService) : ICombinedEntryService
{
    public Task<ApiResponse<IEnumerable<ComplexEntryDto>>> GetComplexEntries()
        => entryService.GetComplexEntries(EntryType.Combined);

    public Task<ApiResponse<ComplexEntryDto>> GetComplexEntryById(Guid id)
        => entryService.GetComplexEntryById(id, EntryType.Combined);

    public Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime)
        => entryService.GetEntryNumber(dateTime);
}
