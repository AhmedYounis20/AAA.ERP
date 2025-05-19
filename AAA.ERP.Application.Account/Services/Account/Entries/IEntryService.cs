using AAA.ERP.OutputDtos;
using Domain.Account.Commands.Entries;
using Domain.Account.Models.Entities.Entries;
using ERP.Application.Services.BaseServices;
using Shared.Responses;

namespace ERP.Application.Services.Account.Entries;

public interface IEntryService : IBaseService<Entry, EntryCreateCommand, EntryUpdateCommand>
{
    Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime);
    Task<ApiResponse<IEnumerable<EntryDto>>> Get(EntryType? entryType = null);
    Task<ApiResponse<EntryDto>> Get(Guid id, EntryType? entryType = null);
}