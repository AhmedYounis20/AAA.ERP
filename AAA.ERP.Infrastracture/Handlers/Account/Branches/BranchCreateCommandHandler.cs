using ERP.Application.Services.Account.SubLeadgers;
using ERP.Domain.Commands.Account.SubLeadgers.Branches;
using ERP.Domain.Models.Entities.Account.SubLeadgers;
using Shared;

namespace ERP.Infrastracture.Handlers.Account.Branches;

public class BranchCreateCommandHandler(IBranchService service) : ICommandHandler<BranchCreateCommand, ApiResponse<Branch>>
{
    public async Task<ApiResponse<Branch>> Handle(BranchCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}