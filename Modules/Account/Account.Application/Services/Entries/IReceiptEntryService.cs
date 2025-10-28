using Account.Domain.Commands.Entries.ReceiptEntries;
using Account.Domain.DTOs;
using Account.Domain.Models.Entities.Entries;
using Shared.Application.BaseServices;
using Shared.Domain.Responses;

namespace Account.Application.Services.Entries;

public interface IReceiptEntryService : IBaseService<Entry, ReceiptEntryCreateCommand, ReceiptEntryUpdateCommand>
{
    Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime);
    Task<ApiResponse<ComplexEntryDto>> GetComplexEntryById(Guid id);
    Task<ApiResponse<IEnumerable<ComplexEntryDto>>> GetComplexEntries();
}