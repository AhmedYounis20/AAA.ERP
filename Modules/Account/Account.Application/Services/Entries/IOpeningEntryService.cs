using Account.Domain.Commands.Entries.OpeningEntries;
using Account.Domain.DTOs;
using Account.Domain.Models.Entities.Entries;
using Shared.Application.BaseServices;
using Shared.Domain.Responses;

namespace Account.Application.Services.Entries;

public interface IOpeningEntryService : IBaseService<Entry, OpeningEntryCreateCommand, OpeningEntryUpdateCommand>
{
    Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime);
    Task<ApiResponse<IEnumerable<EntryDto>>> GetDto();
    Task<ApiResponse<EntryDto>> GetDto(Guid id);
}