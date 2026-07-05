using Accounting.Application.Commands.Branches;
using Accounting.Application.Services;
using ERP.Domain.Models.Entities.Account.SubLeadgers;

namespace Accounting.Infrastructure.Handlers.Branches;

public class BranchUpdateCommandHandler(IBranchService service)
    : ICommandHandler<BranchUpdateCommand, ApiResponse<Branch>>
{
    public Task<ApiResponse<Branch>> Handle(BranchUpdateCommand request, CancellationToken cancellationToken)
        => service.Update(request);
}
