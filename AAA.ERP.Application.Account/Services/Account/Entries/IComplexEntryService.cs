using AAA.ERP.OutputDtos;
using ERP.Application.Services.BaseServices;
using ERP.Domain.Commands.Account.Entries;
using ERP.Domain.Models.Entities.Account.Entries;
using Shared.Responses;

namespace ERP.Application.Services.Account.Entries;

public interface IComplexEntryService : IBaseService<Entry, ComplexEntryCreateCommand, ComplexEntryUpdateCommand>
{
    Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime);
    Task<ApiResponse<ComplexEntryDto>> GetComplexEntryById(Guid id, EntryType? entryType = null);
    Task<ApiResponse<IEnumerable<ComplexEntryDto>>> GetComplexEntries(EntryType? entryType = null);
}