using Domain.Account.Commands.AccountGuides;
using Domain.Account.Models.Entities.AccountGuide;
using ERP.Application.Services.Account;
using Shared;

namespace ERP.Infrastracture.Handlers.AccountGuides;

public class AccountGuideCreateCommandHandler(IAccountGuideService service) : ICommandHandler<AccountGuideCreateCommand, ApiResponse<AccountGuide>>
{
    public async Task<ApiResponse<AccountGuide>> Handle(AccountGuideCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}