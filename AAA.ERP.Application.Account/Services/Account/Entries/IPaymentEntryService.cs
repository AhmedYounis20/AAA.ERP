using AAA.ERP.OutputDtos;
using Domain.Account.Commands.Entries.PaymentEntries;
using Domain.Account.Models.Entities.Entries;
using ERP.Application.Services.BaseServices;
using Shared.Responses;

namespace ERP.Application.Services.Account.Entries;

public interface IPaymentEntryService : IBaseService<Entry, PaymentEntryCreateCommand, PaymentEntryUpdateCommand>
{
    Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime);
    Task<ApiResponse<ComplexEntryDto>> GetComplexEntryById(Guid id);
    Task<ApiResponse<IEnumerable<ComplexEntryDto>>> GetComplexEntries();
}