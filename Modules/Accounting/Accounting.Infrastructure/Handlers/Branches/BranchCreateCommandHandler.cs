using Accounting.Application.Commands.Branches;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.SubLeadgers;

namespace Accounting.Infrastructure.Handlers.Branches;

public sealed class BranchCreateCommandHandler(IBranchWorkflowService workflow)
    : ICommandHandler<BranchCreateCommand, ApiResponse<Branch>>
{
    public Task<ApiResponse<Branch>> Handle(BranchCreateCommand request, CancellationToken cancellationToken)
        => workflow.Create(request);
}

public sealed class BranchUpdateCommandHandler(IBranchWorkflowService workflow)
    : ICommandHandler<BranchUpdateCommand, ApiResponse<Branch>>
{
    public Task<ApiResponse<Branch>> Handle(BranchUpdateCommand request, CancellationToken cancellationToken)
        => workflow.Update(request);
}

public sealed class BranchDeleteCommandHandler(IBranchWorkflowService workflow)
    : ICommandHandler<BranchDeleteCommand, ApiResponse<Branch>>
{
    public Task<ApiResponse<Branch>> Handle(BranchDeleteCommand request, CancellationToken cancellationToken)
        => workflow.Delete(request.Id);
}
