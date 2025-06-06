using ERP.Application.Repositories.Account;
using ERP.Application.Services.Account.Entries;
using ERP.Domain.Commands.Account.Entries;
using ERP.Domain.Commands.Account.Entries.OpeningEntries;
using ERP.Domain.Models.Entities.Account.Entries;

namespace ERP.Infrastracture.Services.Account.Entries;

public class OpeningEntryService(IEntryService _entryService, IEntryRepository _entryRepository, ApplicationDbContext _dbContext)
            : BaseService<Entry, OpeningEntryCreateCommand, OpeningEntryUpdateCommand>(_entryRepository), IOpeningEntryService
{
    public override async Task<ApiResponse<Entry>> Create(OpeningEntryCreateCommand entity, bool isValidate = true)
    {
        var entryCreateCommand = entity.Adapt<EntryCreateCommand>();
        entryCreateCommand.Type = EntryType.Opening;
        return await _entryService.Create(entryCreateCommand, isValidate);
    }

    public override async Task<ApiResponse<Entry>> Update(OpeningEntryUpdateCommand entity, bool isValidate = true)
    {
        var entryUpdateCommand = entity.Adapt<EntryUpdateCommand>();
        return await _entryService.Update(entryUpdateCommand, isValidate);
    }

    public async Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime)
    {
        return await _entryService.GetEntryNumber(dateTime);
    }

    public async Task<ApiResponse<IEnumerable<EntryDto>>> GetDto()
=> await _entryService.Get(EntryType.Opening);

    public async Task<ApiResponse<EntryDto>> GetDto(Guid id)
    => await _entryService.Get(id, EntryType.Opening);
}