using AAA.ERP.OutputDtos;
using ERP.Application.Services.BaseServices;
using ERP.Domain.Commands.Account.Entries.PaymentEntries;
using ERP.Domain.Models.Entities.Account.Entries;
using Shared.Responses;

namespace ERP.Application.Services.Account.Entries;

public interface IPaymentEntryService : IBaseService<Entry, PaymentEntryCreateCommand, PaymentEntryUpdateCommand>
{
    Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime);
    Task<ApiResponse<ComplexEntryDto>> GetComplexEntryById(Guid id);
    Task<ApiResponse<IEnumerable<ComplexEntryDto>>> GetComplexEntries();
}