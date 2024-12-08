using AAA.ERP.Services.Interfaces.SubLeadgers;
using Domain.Account.Commands.Currencies;
using Domain.Account.InputModels.Subleadgers;
using Domain.Account.Models.Entities.Currencies;
using Domain.Account.Models.Entities.Entries;
using Domain.Account.Services.Interfaces;
using Shared;
using Shared.Responses;

namespace Domain.Account.Handlers.Entries;

public class EntryCreateCommandHandler(IEntryService service): ICommandHandler<EntryCreateCommand,ApiResponse<Entry>>
{
    public async Task<ApiResponse<Entry>> Handle(EntryCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}