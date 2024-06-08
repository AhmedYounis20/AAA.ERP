using AAA.ERP.Services.Interfaces.SubLeadgers;
using Domain.Account.Commands.SubLeadgers.CashInBoxes;
using Domain.Account.InputModels.Subleadgers;
using Domain.Account.Models.Entities.SubLeadgers;
using Shared;
using Shared.Responses;

namespace Domain.Account.Handlers.CashInBoxes;

public class CashInBoxCreateCommandHandler(ICashInBoxService service): ICommandHandler<CashInBoxCreateCommand,ApiResponse<CashInBox>>
{
    public async Task<ApiResponse<CashInBox>> Handle(CashInBoxCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}