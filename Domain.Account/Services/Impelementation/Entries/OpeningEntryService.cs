using System.Numerics;
using AAA.ERP.OutputDtos;
using Domain.Account.Commands.Entries;
using Domain.Account.Commands.Entries.OpeningEntries;
using Domain.Account.DBConfiguration.DbContext;
using Domain.Account.Models.Entities.Entries;
using Domain.Account.Models.Entities.FinancialPeriods;
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
        return await _entryService.Create(entryCreateCommand);
    }

    public override async Task<ApiResponse<Entry>> Update(OpeningEntryUpdateCommand entity, bool isValidate = true)
    {
        var entryUpdateCommand = entity.Adapt<EntryUpdateCommand>();
        return await _entryService.Update(entryUpdateCommand);
    }

    public async Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime)
    {
        return await _entryService.GetEntryNumber(dateTime);
    }
}