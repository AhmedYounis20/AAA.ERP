using Accounting.Application.Dtos.Entries;
using Accounting.Application.Dtos.Filters;
using Accounting.Application.Services;
using Accounting.Application.Services.Entries;
using Accounting.Contracts.Dtos;
using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.Responses;
using Accounting.Domain.Models.Entities.Entries;

namespace Accounting.Infrastructure.Services.Entries;

/// <summary>
/// Receipt entry reads. Transitional: delegates to <see cref="IReceiptEntryService"/>.
/// </summary>
public class ReceiptEntryQueries(IReceiptEntryService receiptEntryService) : IReceiptEntryQueries
{
    public Task<ApiResponse<IEnumerable<ComplexEntryDto>>> GetAll()
        => receiptEntryService.GetComplexEntries();

    public Task<ApiResponse<PaginatedResult<Entry>>> GetPaginated(
        EntryFilterDto filter,
        CancellationToken cancellationToken = default)
        => receiptEntryService.ReadAllPaginated(filter, cancellationToken);

    public Task<ApiResponse<ComplexEntryDto>> GetById(Guid id)
        => receiptEntryService.GetComplexEntryById(id);

    public Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime)
        => receiptEntryService.GetEntryNumber(dateTime);
}
