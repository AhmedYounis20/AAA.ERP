using AAA.ERP.OutputDtos;
using Domain.Account.Commands.Entries.JournalEntries;
using Domain.Account.DBConfiguration.DbContext;
using Domain.Account.Models.Entities.Entries;
using Domain.Account.Repositories.Interfaces;
using Domain.Account.Services.BaseServices.impelemtation;
using Domain.Account.Services.Interfaces.Entries;
using Shared.Responses;

namespace Domain.Account.Services.Impelementation.Entries;

public class JournalEntryService(IEntryService _entryService, IEntryRepository _entryRepository, ApplicationDbContext _dbContext)
            : BaseService<Entry, JournalEntryCreateCommand, JournalEntryUpdateCommand>(_entryRepository), IJournalEntryService
{
    public async Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime)
    {
        return await _entryService.GetEntryNumber(dateTime);
    }
}