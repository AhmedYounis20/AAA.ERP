using ERP.Application.Services.Account;
using ERP.Domain.Commands.Account.AccountGuides;
using ERP.Domain.Models.Entities.Account.AccountGuides;
using Shared;

namespace ERP.Infrastracture.Handlers.Account.AccountGuides;

public class AccountGuideUpdateCommandHandler(IAccountGuideService service) : ICommandHandler<AccountGuideUpdateCommand, ApiResponse<AccountGuide>>
{
    public async Task<ApiResponse<AccountGuide>> Handle(AccountGuideUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}