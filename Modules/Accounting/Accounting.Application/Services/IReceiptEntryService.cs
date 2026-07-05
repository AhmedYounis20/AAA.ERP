using Accounting.Domain.OutputDtos;
using Accounting.Contracts.Dtos;
using Accounting.Application.Commands.ReceiptEntries;
using ERP.BuildingBlocks.Application.Services.BaseServices;
using Accounting.Domain.Models.Entities.Entries;
using ERP.BuildingBlocks.Application.Responses;

namespace Accounting.Application.Services;

/// <summary>
/// Application service contract for receipt entry.
/// </summary>
public interface IReceiptEntryService : IBaseService<Entry, ReceiptEntryCreateCommand, ReceiptEntryUpdateCommand>
{
    Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime);
    Task<ApiResponse<ComplexEntryDto>> GetComplexEntryById(Guid id);
    Task<ApiResponse<IEnumerable<ComplexEntryDto>>> GetComplexEntries();
}
