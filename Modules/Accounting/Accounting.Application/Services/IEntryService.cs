using Accounting.Domain.OutputDtos;
using ERP.BuildingBlocks.Application.Services.BaseServices;
using Accounting.Application.Commands.Entries;
using Accounting.Domain.Models.Entities.Entries;
using Shared.Responses;

namespace Accounting.Application.Services;

public interface IEntryService : IBaseService<Entry, EntryCreateCommand, EntryUpdateCommand>
{
    Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime);
    Task<ApiResponse<IEnumerable<EntryDto>>> Get(EntryType? entryType = null);
    Task<ApiResponse<EntryDto>> Get(Guid id, EntryType? entryType = null);
}