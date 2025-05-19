using ERP.Application.Services.Account.SubLeadgers;
using ERP.Domain.Commands.Account.SubLeadgers.Branches;
using ERP.Domain.Models.Entities.Account.SubLeadgers;
using Shared;

namespace ERP.Infrastracture.Handlers.Account.Branches;

public class BranchUpdateCommandHandler(IBranchService service) : ICommandHandler<BranchUpdateCommand, ApiResponse<Branch>>
{
    public async Task<ApiResponse<Branch>> Handle(BranchUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}