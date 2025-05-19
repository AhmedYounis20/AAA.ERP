using ERP.Application.Services.Account;
using ERP.Domain.Commands.Account.AccountGuides;
using ERP.Domain.Models.Entities.Account.AccountGuides;
using Shared;

namespace ERP.Infrastracture.Handlers.Account.AccountGuides;

public class AccountGuideCreateCommandHandler(IAccountGuideService service) : ICommandHandler<AccountGuideCreateCommand, ApiResponse<AccountGuide>>
{
    public async Task<ApiResponse<AccountGuide>> Handle(AccountGuideCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}