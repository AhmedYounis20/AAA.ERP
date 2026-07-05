using Accounting.Application.Dtos.Entries;
using Accounting.Contracts.Dtos;
using ERP.BuildingBlocks.Application.Services.BaseServices;
using Accounting.Application.Commands.Entries;
using Accounting.Domain.Models.Entities.Entries;
using ERP.BuildingBlocks.Application.Responses;

namespace Accounting.Application.Services;

/// <summary>
/// Application service contract for entry.
/// </summary>
public interface IEntryService : IBaseService<Entry, EntryCreateCommand, EntryUpdateCommand>
{
    Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime);
    Task<ApiResponse<IEnumerable<EntryDto>>> Get(EntryType? entryType = null);
    Task<ApiResponse<EntryDto>> Get(Guid id, EntryType? entryType = null);
}
