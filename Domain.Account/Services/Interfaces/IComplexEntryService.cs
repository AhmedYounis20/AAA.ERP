using AAA.ERP.OutputDtos;
using Domain.Account.Commands.Entries;
using Domain.Account.Commands.GLSettings;
using Domain.Account.Models.Entities.Entries;
using Domain.Account.Models.Entities.GLSettings;
using Domain.Account.Services.BaseServices.interfaces;
using Shared.Responses;

namespace Domain.Account.Services.Interfaces;

public interface IComplexEntryService : IBaseService<Entry, ComplexEntryCreateCommand, ComplexEntryUpdateCommand>
{
    Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime);
    Task<ApiResponse<EntryDto>> GetComplexEntryById(Guid id);
}