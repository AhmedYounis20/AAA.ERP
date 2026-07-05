using Accounting.Application.Commands.AccountGuides;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.AccountGuides;

namespace Accounting.Infrastructure.Handlers.AccountGuides;

public class AccountGuideCreateCommandHandler(IAccountGuideService service)
    : ICommandHandler<AccountGuideCreateCommand, ApiResponse<AccountGuide>>
{
    public Task<ApiResponse<AccountGuide>> Handle(AccountGuideCreateCommand request, CancellationToken cancellationToken)
        => service.Create(request);
}
