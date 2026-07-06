using Accounting.Application.Dtos.Entries;
using Accounting.Contracts.Dtos;
using Accounting.Application.Commands.CombinedEntries;
using Accounting.Domain.Models.Entities.Entries;
using ERP.BuildingBlocks.Application.Responses;

namespace Accounting.Application.Services;

/// <summary>
/// Combined entry read-side helper. CRUD writes are handled by CQRS workflow.
/// </summary>
public interface ICombinedEntryService
{
    Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime);

    Task<ApiResponse<ComplexEntryDto>> GetComplexEntryById(Guid id);

    Task<ApiResponse<IEnumerable<ComplexEntryDto>>> GetComplexEntries();
}
