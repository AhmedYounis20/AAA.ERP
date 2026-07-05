using Accounting.Application.Dtos.Entries;
using Accounting.Contracts.Dtos;
using ERP.BuildingBlocks.Application.Services.BaseServices;
using Accounting.Application.Commands.OpeningEntries;
using Accounting.Domain.Models.Entities.Entries;
using ERP.BuildingBlocks.Application.Responses;

namespace Accounting.Application.Services;

/// <summary>
/// Application service contract for opening entry.
/// </summary>
public interface IOpeningEntryService : IBaseService<Entry, OpeningEntryCreateCommand, OpeningEntryUpdateCommand>
{
    Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime);
    Task<ApiResponse<IEnumerable<EntryDto>>> GetDto();
    Task<ApiResponse<EntryDto>> GetDto(Guid id);
}
