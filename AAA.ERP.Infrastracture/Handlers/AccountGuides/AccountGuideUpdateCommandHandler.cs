using Domain.Account.Commands.AccountGuides;
using Domain.Account.Models.Entities.AccountGuide;
using ERP.Application.Services.Account;
using Shared;

namespace ERP.Infrastracture.Handlers.AccountGuides;

public class AccountGuideUpdateCommandHandler(IAccountGuideService service) : ICommandHandler<AccountGuideUpdateCommand, ApiResponse<AccountGuide>>
{
    public async Task<ApiResponse<AccountGuide>> Handle(AccountGuideUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}