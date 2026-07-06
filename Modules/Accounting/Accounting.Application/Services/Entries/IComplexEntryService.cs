using Accounting.Application.Dtos.Entries;
using Accounting.Contracts.Dtos;
using Accounting.Application.Commands.Entries;
using Accounting.Domain.Models.Entities.Entries;
using ERP.BuildingBlocks.Application.Responses;

namespace Accounting.Application.Services;

/// <summary>
/// Complex entry read-side helper. CRUD writes are handled by CQRS workflow.
/// </summary>
public interface IComplexEntryService
{
    Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime);

    Task<ApiResponse<ComplexEntryDto>> GetComplexEntryById(Guid id, EntryType? entryType = null);

    Task<ApiResponse<IEnumerable<ComplexEntryDto>>> GetComplexEntries(EntryType? entryType = null);
}
