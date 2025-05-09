using AAA.ERP.Services.Interfaces.SubLeadgers;
using Domain.Account.Commands.Currencies;
using Domain.Account.Commands.SubLeadgers.CashInBoxes;
using Domain.Account.InputModels.Subleadgers;
using Domain.Account.Models.Entities.CostCenters;
using Domain.Account.Services.Interfaces;
using Shared;
using Shared.Responses;

namespace Domain.Account.Handlers.Currencies;

public class CostCenterUpdateCommandHandler(ICostCenterService service): ICommandHandler<CostCenterUpdateCommand,ApiResponse<CostCenter>>
{
    public async Task<ApiResponse<CostCenter>> Handle(CostCenterUpdateCommand request, CancellationToken cancellationToken)
    {
         return await service.Update(request);
    }
}