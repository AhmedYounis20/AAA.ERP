using Accounting.Application.Commands.AccountGuides;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.AccountGuides;

namespace Accounting.Infrastructure.Handlers.AccountGuides;

public class AccountGuideDeleteCommandHandler(IAccountGuideService service)
    : ICommandHandler<AccountGuideDeleteCommand, ApiResponse<AccountGuide>>
{
    public Task<ApiResponse<AccountGuide>> Handle(AccountGuideDeleteCommand request, CancellationToken cancellationToken)
        => service.Delete(request.Id);
}
