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
/// Payment entry reads. Transitional: delegates to <see cref="IPaymentEntryService"/>.
/// </summary>
public class PaymentEntryQueries(IPaymentEntryService paymentEntryService) : IPaymentEntryQueries
{
    public Task<ApiResponse<IEnumerable<ComplexEntryDto>>> GetAll()
        => paymentEntryService.GetComplexEntries();

    public Task<ApiResponse<PaginatedResult<Entry>>> GetPaginated(
        EntryFilterDto filter,
        CancellationToken cancellationToken = default)
        => paymentEntryService.ReadAllPaginated(filter, cancellationToken);

    public Task<ApiResponse<ComplexEntryDto>> GetById(Guid id)
        => paymentEntryService.GetComplexEntryById(id);

    public Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime)
        => paymentEntryService.GetEntryNumber(dateTime);
}
