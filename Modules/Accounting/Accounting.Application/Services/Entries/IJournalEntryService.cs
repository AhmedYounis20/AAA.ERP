using Accounting.Application.Dtos.Entries;
using Accounting.Contracts.Dtos;
using Accounting.Application.Commands.JournalEntries;
using Accounting.Domain.Models.Entities.Entries;
using ERP.BuildingBlocks.Application.Responses;

namespace Accounting.Application.Services;

/// <summary>
/// Journal entry read-side helper. CRUD writes are handled by CQRS workflow.
/// </summary>
public interface IJournalEntryService
{
    Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime);

    Task<ApiResponse<IEnumerable<EntryDto>>> GetDto();

    Task<ApiResponse<EntryDto>> GetDto(Guid id);
}
