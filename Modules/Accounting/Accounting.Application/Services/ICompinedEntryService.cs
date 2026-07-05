using AAA.ERP.OutputDtos;
using ERP.Application.Services.BaseServices;
using Accounting.Application.Commands.CompinedEntries;
using ERP.Domain.Models.Entities.Account.Entries;
using Shared.Responses;

namespace Accounting.Application.Services;

public interface ICompinedEntryService : IBaseService<Entry, CompinedEntryCreateCommand, CompinedEntryUpdateCommand>
{
    Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime);
    Task<ApiResponse<ComplexEntryDto>> GetComplexEntryById(Guid id);
    Task<ApiResponse<IEnumerable<ComplexEntryDto>>> GetComplexEntries();
}