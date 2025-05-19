using Domain.Account.Commands.Entries;
using Domain.Account.Commands.Entries.CompinedEntries;
using Domain.Account.Models.Entities.Entries;
using ERP.Application.Services.Account.Entries;

namespace ERP.Infrastracture.Services.Account.Entries;

public class CompinedEntryService(IComplexEntryService _entryService, IEntryRepository _entryRepository)
            : BaseService<Entry, CompinedEntryCreateCommand, CompinedEntryUpdateCommand>(_entryRepository), ICompinedEntryService
{
    public override async Task<ApiResponse<Entry>> Create(CompinedEntryCreateCommand entity, bool isValidate = true)
    {
        var complexEntry = entity.Adapt<ComplexEntryCreateCommand>();
        complexEntry.Type = EntryType.Compined;
        return await _entryService.Create(complexEntry, isValidate);
    }

    public async Task<ApiResponse<IEnumerable<ComplexEntryDto>>> GetComplexEntries()
    => await _entryService.GetComplexEntries(EntryType.Compined);

    public async Task<ApiResponse<ComplexEntryDto>> GetComplexEntryById(Guid id)
    => await _entryService.GetComplexEntryById(id, EntryType.Compined);

    public async Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime)
    {
        return await _entryService.GetEntryNumber(dateTime);
    }

    public override async Task<ApiResponse<Entry>> Update(CompinedEntryUpdateCommand entity, bool isValidate = true)
    {
        var complexEntry = entity.Adapt<ComplexEntryUpdateCommand>();
        return await _entryService.Update(complexEntry, isValidate);
    }
}