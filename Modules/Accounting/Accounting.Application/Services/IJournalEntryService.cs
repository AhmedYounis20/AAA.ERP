using AAA.ERP.OutputDtos;
using Accounting.Application.Commands.JournalEntries;
using ERP.Application.Services.BaseServices;
using ERP.Domain.Models.Entities.Account.Entries;
using Shared.Responses;

namespace Accounting.Application.Services;

public interface IJournalEntryService : IBaseService<Entry, JournalEntryCreateCommand, JournalEntryUpdateCommand>
{
    Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime);
    Task<ApiResponse<IEnumerable<EntryDto>>> GetDto();
    Task<ApiResponse<EntryDto>> GetDto(Guid id);
}
