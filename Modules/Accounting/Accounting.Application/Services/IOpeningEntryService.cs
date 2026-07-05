using Accounting.Domain.OutputDtos;
using ERP.BuildingBlocks.Application.Services.BaseServices;
using Accounting.Application.Commands.OpeningEntries;
using Accounting.Domain.Models.Entities.Entries;
using Shared.Responses;

namespace Accounting.Application.Services;

public interface IOpeningEntryService : IBaseService<Entry, OpeningEntryCreateCommand, OpeningEntryUpdateCommand>
{
    Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime);
    Task<ApiResponse<IEnumerable<EntryDto>>> GetDto();
    Task<ApiResponse<EntryDto>> GetDto(Guid id);
}