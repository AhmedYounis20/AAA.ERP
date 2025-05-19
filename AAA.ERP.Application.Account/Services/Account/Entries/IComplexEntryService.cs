using AAA.ERP.OutputDtos;
using Domain.Account.Commands.Entries;
using Domain.Account.Models.Entities.Entries;
using ERP.Application.Services.BaseServices;
using Shared.Responses;

namespace ERP.Application.Services.Account.Entries;

public interface IComplexEntryService : IBaseService<Entry, ComplexEntryCreateCommand, ComplexEntryUpdateCommand>
{
    Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime);
    Task<ApiResponse<ComplexEntryDto>> GetComplexEntryById(Guid id, EntryType? entryType = null);
    Task<ApiResponse<IEnumerable<ComplexEntryDto>>> GetComplexEntries(EntryType? entryType = null);
}