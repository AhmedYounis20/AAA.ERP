using AAA.ERP.OutputDtos;
using Domain.Account.Commands.Entries.OpeningEntries;
using Domain.Account.Models.Entities.Entries;
using ERP.Application.Services.BaseServices;
using Shared.Responses;

namespace ERP.Application.Services.Account.Entries;

public interface IOpeningEntryService : IBaseService<Entry, OpeningEntryCreateCommand, OpeningEntryUpdateCommand>
{
    Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime);
    Task<ApiResponse<IEnumerable<EntryDto>>> GetDto();
    Task<ApiResponse<EntryDto>> GetDto(Guid id);
}