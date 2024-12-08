using AAA.ERP.Services.Interfaces.SubLeadgers;
using Domain.Account.Commands.Currencies;
using Domain.Account.Commands.SubLeadgers.CashInBoxes;
using Domain.Account.InputModels.Subleadgers;
using Domain.Account.Models.Entities.Currencies;
using Domain.Account.Models.Entities.Entries;
using Domain.Account.Services.Interfaces;
using Shared;
using Shared.Responses;

namespace Domain.Account.Handlers.Currencies;

public class EntryUpdateCommandHandler(IEntryService service): ICommandHandler<EntryUpdateCommand,ApiResponse<Entry>>
{
    public async Task<ApiResponse<Entry>> Handle(EntryUpdateCommand request, CancellationToken cancellationToken)
    {
         return await service.Update(request);
    }
}