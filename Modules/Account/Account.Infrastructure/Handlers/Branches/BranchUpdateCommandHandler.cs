using Account.Application.Services.SubLeadgers;
using Account.Domain.Commands.SubLeadgers.Branches;
using Account.Domain.Models.Entities.SubLeadgers;

namespace Account.Infrastructure.Handlers.Branches;

public class BranchUpdateCommandHandler(IBranchService service) : ICommandHandler<BranchUpdateCommand, ApiResponse<Branch>>
{
    public async Task<ApiResponse<Branch>> Handle(BranchUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}
