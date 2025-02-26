using AAA.ERP.Services.Interfaces.SubLeadgers;
using Domain.Account.Commands.Entries;
using Domain.Account.Commands.SubLeadgers.CashInBoxes;
using Domain.Account.InputModels.Subleadgers;
using Domain.Account.Models.Entities.Entries;
using Domain.Account.Services.Interfaces;
using Shared;
using Shared.Responses;

namespace Domain.Account.Handlers.Entries;

public class EntryUpdateCommandHandler(IComplexEntryService service): ICommandHandler<ComplexEntryUpdateCommand,ApiResponse<Entry>>
{
    public async Task<ApiResponse<Entry>> Handle(ComplexEntryUpdateCommand request, CancellationToken cancellationToken)
    {
         return await service.Update(request);
    }
}