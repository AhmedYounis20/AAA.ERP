using Account.Application.Services.SubLeadgers;
using Account.Domain.Commands.SubLeadgers.Branches;
using Account.Domain.Models.Entities.SubLeadgers;

namespace Account.Infrastructure.Handlers.Branches;

public class BranchCreateCommandHandler(IBranchService service) : ICommandHandler<BranchCreateCommand, ApiResponse<Branch>>
{
    public async Task<ApiResponse<Branch>> Handle(BranchCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}
