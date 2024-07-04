using Domain.Account.Commands.SubLeadgers.Banks;
using Domain.Account.Commands.SubLeadgers.CashInBoxes;
using Domain.Account.Commands.SubLeadgers.FixedAssets;
using Domain.Account.Models.Entities.SubLeadgers;
using Domain.Account.Services.Interfaces.SubLeadgers;
using Shared;
using Shared.Responses;

namespace Domain.Account.Handlers.Banks;

public class FixedAssetCreateCommandHandler(IFixedAssetService service): ICommandHandler<FixedAssetCreateCommand,ApiResponse<FixedAsset>>
{
    public async Task<ApiResponse<FixedAsset>> Handle(FixedAssetCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}