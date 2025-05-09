using AAA.ERP.OutputDtos;
using Domain.Account.Commands.Entries.ReceiptEntries;
using Domain.Account.Models.Entities.Entries;
using Domain.Account.Services.BaseServices.interfaces;
using Shared.Responses;

namespace Domain.Account.Services.Interfaces.Entries;

public interface IReceiptEntryService : IBaseService<Entry, ReceiptEntryCreateCommand, ReceiptEntryUpdateCommand>
{
    Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime);
    Task<ApiResponse<ComplexEntryDto>> GetComplexEntryById(Guid id);
    Task<ApiResponse<IEnumerable<ComplexEntryDto>>> GetComplexEntries();
}