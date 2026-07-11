using Accounting.Application.Dtos.Entries;
using Accounting.Application.Dtos.Filters;
using Accounting.Contracts.Dtos;
using Accounting.Application.Commands.Entries;
using Accounting.Domain.Models.Entities.Entries;
using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.Responses;

namespace Accounting.Application.Services;

/// <summary>
/// Entry read-side helper. CRUD writes are handled by CQRS workflow.
/// </summary>
public interface IEntryService
{
    Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime);

    Task<ApiResponse<IEnumerable<EntryDto>>> Get(EntryType? entryType = null);

    Task<ApiResponse<EntryDto>> Get(Guid id, EntryType? entryType = null);

    Task<ApiResponse<IEnumerable<Entry>>> ReadAll();

    Task<ApiResponse<PaginatedResult<Entry>>> GetPaginated(
        EntryFilterDto filter,
        CancellationToken cancellationToken = default);
}
