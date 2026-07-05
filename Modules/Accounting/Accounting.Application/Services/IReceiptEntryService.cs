using AAA.ERP.OutputDtos;
using Accounting.Application.Commands.ReceiptEntries;
using ERP.BuildingBlocks.Application.Services.BaseServices;
using Accounting.Domain.Models.Entities.Entries;
using Shared.Responses;

namespace Accounting.Application.Services;

public interface IReceiptEntryService : IBaseService<Entry, ReceiptEntryCreateCommand, ReceiptEntryUpdateCommand>
{
    Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime);
    Task<ApiResponse<ComplexEntryDto>> GetComplexEntryById(Guid id);
    Task<ApiResponse<IEnumerable<ComplexEntryDto>>> GetComplexEntries();
}
