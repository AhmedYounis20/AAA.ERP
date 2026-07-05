using AAA.ERP.OutputDtos;
using Accounting.Application.Commands.PaymentEntries;
using ERP.Application.Services.BaseServices;
using ERP.Domain.Models.Entities.Account.Entries;
using Shared.Responses;

namespace Accounting.Application.Services;

public interface IPaymentEntryService : IBaseService<Entry, PaymentEntryCreateCommand, PaymentEntryUpdateCommand>
{
    Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime);
    Task<ApiResponse<ComplexEntryDto>> GetComplexEntryById(Guid id);
    Task<ApiResponse<IEnumerable<ComplexEntryDto>>> GetComplexEntries();
}
