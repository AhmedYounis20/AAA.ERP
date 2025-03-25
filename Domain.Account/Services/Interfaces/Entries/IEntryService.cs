using AAA.ERP.OutputDtos;
using Domain.Account.Commands.Entries;
using Domain.Account.Models.Entities.Entries;
using Domain.Account.Services.BaseServices.interfaces;
using Shared.Responses;

namespace Domain.Account.Services.Interfaces.Entries;

public interface IEntryService : IBaseService<Entry, EntryCreateCommand, EntryUpdateCommand>
{
    Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime);
}