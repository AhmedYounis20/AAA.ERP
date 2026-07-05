using Accounting.Domain.OutputDtos;
using Accounting.Contracts.Dtos;
using ERP.BuildingBlocks.Application.Services.BaseServices;
using Accounting.Application.Commands.CombinedEntries;
using Accounting.Domain.Models.Entities.Entries;
using ERP.BuildingBlocks.Application.Responses;

namespace Accounting.Application.Services;

/// <summary>
/// Application service contract for combined entry.
/// </summary>
public interface ICombinedEntryService : IBaseService<Entry, CombinedEntryCreateCommand, CombinedEntryUpdateCommand>
{
    Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime);
    Task<ApiResponse<ComplexEntryDto>> GetComplexEntryById(Guid id);
    Task<ApiResponse<IEnumerable<ComplexEntryDto>>> GetComplexEntries();
}
