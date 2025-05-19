using Domain.Account.Commands.SubLeadgers.CashInBoxes;
using Domain.Account.Models.Entities.SubLeadgers;
using ERP.Application.Services.Account.SubLeadgers;
using Shared;

namespace ERP.Infrastracture.Handlers.Branches;

public class BranchCreateCommandHandler(IBranchService service) : ICommandHandler<BranchCreateCommand, ApiResponse<Branch>>
{
    public async Task<ApiResponse<Branch>> Handle(BranchCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}