using AAA.ERP.OutputDtos;
using ERP.Application.Services.BaseServices;
using Accounting.Application.Commands.CombinedEntries;
using ERP.Domain.Models.Entities.Account.Entries;
using Shared.Responses;

namespace Accounting.Application.Services;

public interface ICombinedEntryService : IBaseService<Entry, CombinedEntryCreateCommand, CombinedEntryUpdateCommand>
{
    Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime);
    Task<ApiResponse<ComplexEntryDto>> GetComplexEntryById(Guid id);
    Task<ApiResponse<IEnumerable<ComplexEntryDto>>> GetComplexEntries();
}