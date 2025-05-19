using AAA.ERP.OutputDtos;
using Domain.Account.Commands.Entries.CompinedEntries;
using Domain.Account.Models.Entities.Entries;
using ERP.Application.Services.BaseServices;
using Shared.Responses;

namespace ERP.Application.Services.Account.Entries;

public interface ICompinedEntryService : IBaseService<Entry, CompinedEntryCreateCommand, CompinedEntryUpdateCommand>
{
    Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime);
    Task<ApiResponse<ComplexEntryDto>> GetComplexEntryById(Guid id);
    Task<ApiResponse<IEnumerable<ComplexEntryDto>>> GetComplexEntries();
}