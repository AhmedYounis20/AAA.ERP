using Domain.Account.Commands.Entries;
using Domain.Account.Commands.Entries.JournalEntries;
using Domain.Account.Models.Entities.Entries;
using ERP.Application.Services.Account.Entries;

namespace ERP.Infrastracture.Services.Account.Entries;

public class JournalEntryService(IEntryService _entryService, IEntryRepository _entryRepository, ApplicationDbContext _dbContext)
            : BaseService<Entry, JournalEntryCreateCommand, JournalEntryUpdateCommand>(_entryRepository), IJournalEntryService
{
    public override async Task<ApiResponse<Entry>> Create(JournalEntryCreateCommand entity, bool isValidate = true)
    {
        var entryCreateCommand = entity.Adapt<EntryCreateCommand>();
        entryCreateCommand.Type = EntryType.Journal;
        return await _entryService.Create(entryCreateCommand, isValidate);
    }

    public override async Task<ApiResponse<Entry>> Update(JournalEntryUpdateCommand entity, bool isValidate = true)
    {
        var entryUpdateCommand = entity.Adapt<EntryUpdateCommand>();
        return await _entryService.Update(entryUpdateCommand, isValidate);
    }

    public async Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime)
    {
        return await _entryService.GetEntryNumber(dateTime);
    }

    public async Task<ApiResponse<IEnumerable<EntryDto>>> GetDto()
    => await _entryService.Get(EntryType.Journal);

    public async Task<ApiResponse<EntryDto>> GetDto(Guid id)
    => await _entryService.Get(id, EntryType.Journal);

}