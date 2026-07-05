using Accounting.Application.Commands.AccountGuides;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.AccountGuides;

namespace Accounting.Infrastructure.Handlers.AccountGuides;

public class AccountGuideUpdateCommandHandler(IAccountGuideService service)
    : ICommandHandler<AccountGuideUpdateCommand, ApiResponse<AccountGuide>>
{
    public Task<ApiResponse<AccountGuide>> Handle(AccountGuideUpdateCommand request, CancellationToken cancellationToken)
        => service.Update(request);
}
