using Accounting.Contracts.Dtos;
using Accounting.Application.Dtos.Entries;
using Accounting.Application.Repositories;
using Accounting.Application.Services;
using Accounting.Application.Commands.Entries;
using Accounting.Application.Commands.OpeningEntries;
using Accounting.Domain.Models.Entities.Entries;
using ERP.Infrastructure.Services.BaseServices;

namespace Accounting.Infrastructure.Services;

public class OpeningEntryService(IEntryService entryService, IEntryRepository entryRepository)
            : BaseService<Entry, OpeningEntryCreateCommand, OpeningEntryUpdateCommand>(entryRepository), IOpeningEntryService
{
    public override async Task<ApiResponse<Entry>> Create(OpeningEntryCreateCommand entity, bool isValidate = true)
    {
        var entryCreateCommand = entity.Adapt<EntryCreateCommand>();
        entryCreateCommand.Type = EntryType.Opening;
        return await entryService.Create(entryCreateCommand, isValidate);
    }

    public override async Task<ApiResponse<Entry>> Update(OpeningEntryUpdateCommand entity, bool isValidate = true)
    {
        var entryUpdateCommand = entity.Adapt<EntryUpdateCommand>();
        return await entryService.Update(entryUpdateCommand, isValidate);
    }

    public async Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime)
    {
        return await entryService.GetEntryNumber(dateTime);
    }

    public async Task<ApiResponse<IEnumerable<EntryDto>>> GetDto()
=> await entryService.Get(EntryType.Opening);

    public async Task<ApiResponse<EntryDto>> GetDto(Guid id)
    => await entryService.Get(id, EntryType.Opening);
}