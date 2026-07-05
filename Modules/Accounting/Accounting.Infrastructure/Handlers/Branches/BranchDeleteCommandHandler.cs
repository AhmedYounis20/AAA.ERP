using Accounting.Application.Commands.Branches;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.SubLeadgers;

namespace Accounting.Infrastructure.Handlers.Branches;

public class BranchDeleteCommandHandler(IBranchService branchService)
    : ICommandHandler<BranchDeleteCommand, ApiResponse<Branch>>
{
    public Task<ApiResponse<Branch>> Handle(BranchDeleteCommand request, CancellationToken cancellationToken)
        => branchService.Delete(request.Id);
}
