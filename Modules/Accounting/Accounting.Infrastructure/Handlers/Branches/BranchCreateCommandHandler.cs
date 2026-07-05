using Accounting.Application.Commands.Branches;
using Accounting.Application.Services;
using ERP.Domain.Models.Entities.Account.SubLeadgers;

namespace Accounting.Infrastructure.Handlers.Branches;

public class BranchCreateCommandHandler(IBranchService service)
    : ICommandHandler<BranchCreateCommand, ApiResponse<Branch>>
{
    public Task<ApiResponse<Branch>> Handle(BranchCreateCommand request, CancellationToken cancellationToken)
        => service.Create(request);
}
