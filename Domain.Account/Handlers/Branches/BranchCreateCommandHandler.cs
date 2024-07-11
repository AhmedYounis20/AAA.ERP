using AAA.ERP.Services.Interfaces.SubLeadgers;
using Domain.Account.Commands.SubLeadgers.CashInBoxes;
using Domain.Account.InputModels.Subleadgers;
using Domain.Account.Models.Entities.SubLeadgers;
using Shared;
using Shared.Responses;

namespace Domain.Account.Handlers.CashInBoxes;

public class BranchCreateCommandHandler(IBranchService service): ICommandHandler<BranchCreateCommand,ApiResponse<Branch>>
{
    public async Task<ApiResponse<Branch>> Handle(BranchCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}