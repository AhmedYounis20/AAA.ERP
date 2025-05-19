using AAA.ERP.OutputDtos;
using ERP.Application.Services.BaseServices;
using ERP.Domain.Commands.Account.Entries;
using ERP.Domain.Models.Entities.Account.Entries;
using Shared.Responses;

namespace ERP.Application.Services.Account.Entries;

public interface IEntryService : IBaseService<Entry, EntryCreateCommand, EntryUpdateCommand>
{
    Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime);
    Task<ApiResponse<IEnumerable<EntryDto>>> Get(EntryType? entryType = null);
    Task<ApiResponse<EntryDto>> Get(Guid id, EntryType? entryType = null);
}