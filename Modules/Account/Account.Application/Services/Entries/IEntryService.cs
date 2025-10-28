using Account.Domain.Commands.Entries;
using Account.Domain.DTOs;
using Account.Domain.Models.Entities.Entries;
using Shared.Application.BaseServices;
using Shared.Domain.Responses;

namespace Account.Application.Services.Entries;

public interface IEntryService : IBaseService<Entry, EntryCreateCommand, EntryUpdateCommand>
{
    Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime);
    Task<ApiResponse<IEnumerable<EntryDto>>> Get(EntryType? entryType = null);
    Task<ApiResponse<EntryDto>> Get(Guid id, EntryType? entryType = null);
}