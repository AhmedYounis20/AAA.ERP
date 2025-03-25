using AAA.ERP.Services.Interfaces.SubLeadgers;
using Domain.Account.Commands.Entries;
using Domain.Account.InputModels.Subleadgers;
using Domain.Account.Models.Entities.Entries;
using Domain.Account.Services.Interfaces;
using Domain.Account.Services.Interfaces.Entries;
using Shared;
using Shared.Responses;

namespace Domain.Account.Handlers.Entries;

public class EntryCreateCommandHandler(IComplexEntryService service): ICommandHandler<ComplexEntryCreateCommand,ApiResponse<Entry>>
{
    public async Task<ApiResponse<Entry>> Handle(ComplexEntryCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}