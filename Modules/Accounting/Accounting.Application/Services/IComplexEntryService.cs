using AAA.ERP.OutputDtos;
using ERP.Application.Services.BaseServices;
using Accounting.Application.Commands.Entries;
using ERP.Domain.Models.Entities.Account.Entries;
using Shared.Responses;

namespace Accounting.Application.Services;

public interface IComplexEntryService : IBaseService<Entry, ComplexEntryCreateCommand, ComplexEntryUpdateCommand>
{
    Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime);
    Task<ApiResponse<ComplexEntryDto>> GetComplexEntryById(Guid id, EntryType? entryType = null);
    Task<ApiResponse<IEnumerable<ComplexEntryDto>>> GetComplexEntries(EntryType? entryType = null);
}