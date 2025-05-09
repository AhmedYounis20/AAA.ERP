using AAA.ERP.OutputDtos;
using Domain.Account.Commands.Entries;
using Domain.Account.Models.Entities.Entries;
using Domain.Account.Services.BaseServices.interfaces;
using Shared.Responses;

namespace Domain.Account.Services.Interfaces.Entries;

public interface IComplexEntryService : IBaseService<Entry, ComplexEntryCreateCommand, ComplexEntryUpdateCommand>
{
    Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime);
    Task<ApiResponse<ComplexEntryDto>> GetComplexEntryById(Guid id,EntryType? entryType = null);
    Task<ApiResponse<IEnumerable<ComplexEntryDto>>> GetComplexEntries(EntryType? entryType = null);
}