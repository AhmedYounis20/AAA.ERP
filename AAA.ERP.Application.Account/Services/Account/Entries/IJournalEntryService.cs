using AAA.ERP.OutputDtos;
using ERP.Application.Services.BaseServices;
using ERP.Domain.Commands.Account.Entries.JournalEntries;
using ERP.Domain.Models.Entities.Account.Entries;
using Shared.Responses;

namespace ERP.Application.Services.Account.Entries;

public interface IJournalEntryService : IBaseService<Entry, JournalEntryCreateCommand, JournalEntryUpdateCommand>
{
    Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime);
    Task<ApiResponse<IEnumerable<EntryDto>>> GetDto();
    Task<ApiResponse<EntryDto>> GetDto(Guid id);
}