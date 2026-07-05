using Accounting.Contracts.Dtos;
using Accounting.Application.Dtos.Entries;
using Accounting.Application.Commands.JournalEntries;
using Accounting.Application.Commands.Entries;
using Accounting.Application.Repositories;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.Entries;
using ERP.Infrastructure.Services.BaseServices;

namespace Accounting.Infrastructure.Services;

public class JournalEntryService(IEntryService entryService, IEntryRepository entryRepository)
    : BaseService<Entry, JournalEntryCreateCommand, JournalEntryUpdateCommand>(entryRepository), IJournalEntryService
{
    public override async Task<ApiResponse<Entry>> Create(JournalEntryCreateCommand entity, bool isValidate = true)
    {
        var entryCreateCommand = entity.Adapt<EntryCreateCommand>();
        entryCreateCommand.Type = EntryType.Journal;
        return await entryService.Create(entryCreateCommand, isValidate);
    }

    public override async Task<ApiResponse<Entry>> Update(JournalEntryUpdateCommand entity, bool isValidate = true)
    {
        var entryUpdateCommand = entity.Adapt<EntryUpdateCommand>();
        return await entryService.Update(entryUpdateCommand, isValidate);
    }

    public Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime)
        => entryService.GetEntryNumber(dateTime);

    public Task<ApiResponse<IEnumerable<EntryDto>>> GetDto()
        => entryService.Get(EntryType.Journal);

    public Task<ApiResponse<EntryDto>> GetDto(Guid id)
        => entryService.Get(id, EntryType.Journal);
}
