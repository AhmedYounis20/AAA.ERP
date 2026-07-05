using Accounting.Domain.OutputDtos;
using Accounting.Application.Commands.JournalEntries;
using ERP.BuildingBlocks.Application.Services.BaseServices;
using Accounting.Domain.Models.Entities.Entries;
using Shared.Responses;

namespace Accounting.Application.Services;

/// <summary>
/// Application service contract for journal entry.
/// </summary>
public interface IJournalEntryService : IBaseService<Entry, JournalEntryCreateCommand, JournalEntryUpdateCommand>
{
    Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime);
    Task<ApiResponse<IEnumerable<EntryDto>>> GetDto();
    Task<ApiResponse<EntryDto>> GetDto(Guid id);
}
