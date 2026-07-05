using Accounting.Domain.OutputDtos;
using Accounting.Contracts.Dtos;
using Accounting.Application.Commands.PaymentEntries;
using ERP.BuildingBlocks.Application.Services.BaseServices;
using Accounting.Domain.Models.Entities.Entries;
using ERP.BuildingBlocks.Application.Responses;

namespace Accounting.Application.Services;

/// <summary>
/// Application service contract for payment entry.
/// </summary>
public interface IPaymentEntryService : IBaseService<Entry, PaymentEntryCreateCommand, PaymentEntryUpdateCommand>
{
    Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime);
    Task<ApiResponse<ComplexEntryDto>> GetComplexEntryById(Guid id);
    Task<ApiResponse<IEnumerable<ComplexEntryDto>>> GetComplexEntries();
}
