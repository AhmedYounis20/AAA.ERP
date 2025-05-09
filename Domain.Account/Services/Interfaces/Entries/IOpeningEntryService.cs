using AAA.ERP.OutputDtos;
using Domain.Account.Commands.Entries.OpeningEntries;
using Domain.Account.Models.Entities.Entries;
using Domain.Account.Services.BaseServices.interfaces;
using Shared.Responses;

namespace Domain.Account.Services.Interfaces.Entries;

public interface IOpeningEntryService : IBaseService<Entry, OpeningEntryCreateCommand, OpeningEntryUpdateCommand>
{
    Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime);
    Task<ApiResponse<IEnumerable<EntryDto>>> GetDto();
    Task<ApiResponse<EntryDto>> GetDto(Guid id);
}