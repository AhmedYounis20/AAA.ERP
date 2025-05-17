using AAA.ERP.OutputDtos;
using Domain.Account.Commands.Entries.CompinedEntries;
using Domain.Account.Models.Entities.Entries;
using Domain.Account.Services.BaseServices.interfaces;
using Shared.Responses;

namespace Domain.Account.Services.Interfaces.Entries;

public interface ICompinedEntryService : IBaseService<Entry, CompinedEntryCreateCommand, CompinedEntryUpdateCommand>
{
    Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime);
    Task<ApiResponse<ComplexEntryDto>> GetComplexEntryById(Guid id);
    Task<ApiResponse<IEnumerable<ComplexEntryDto>>> GetComplexEntries();
}