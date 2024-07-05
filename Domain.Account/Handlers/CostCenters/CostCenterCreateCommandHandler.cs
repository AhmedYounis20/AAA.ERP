using AAA.ERP.Services.Interfaces.SubLeadgers;
using Domain.Account.Commands.Currencies;
using Domain.Account.InputModels.Subleadgers;
using Domain.Account.Models.Entities.Currencies;
using Domain.Account.Services.Interfaces;
using Shared;
using Shared.Responses;

namespace Domain.Account.Handlers.Currencies;

public class CostCenterCreateCommandHandler(ICostCenterService service): ICommandHandler<CostCenterCreateCommand,ApiResponse<CostCenter>>
{
    public async Task<ApiResponse<CostCenter>> Handle(CostCenterCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}