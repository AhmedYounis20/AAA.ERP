using AAA.ERP.OutputDtos;
using Domain.Account.Commands.Entries;
using Domain.Account.Commands.Entries.OpeningEntries;
using Domain.Account.DBConfiguration.DbContext;
using Domain.Account.Models.Entities.Entries;
using Domain.Account.Repositories.Interfaces;
using Domain.Account.Services.BaseServices.impelemtation;
using Domain.Account.Services.Interfaces.Entries;
using Mapster;
using Shared.Responses;

namespace Domain.Account.Services.Impelementation.Entries;

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