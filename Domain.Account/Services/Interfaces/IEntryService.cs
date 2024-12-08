using AAA.ERP.OutputDtos;
using Domain.Account.Commands.Currencies;
using Domain.Account.Commands.GLSettings;
using Domain.Account.Models.Entities.Entries;
using Domain.Account.Models.Entities.GLSettings;
using Domain.Account.Services.BaseServices.interfaces;
using Shared.Responses;

namespace Domain.Account.Services.Interfaces;

public interface IEntryService : IBaseService<Entry, EntryCreateCommand, EntryUpdateCommand>
{
    Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime);

}