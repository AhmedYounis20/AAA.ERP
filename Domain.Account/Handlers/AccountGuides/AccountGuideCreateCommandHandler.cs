using AAA.ERP.Services.Interfaces.SubLeadgers;
using Domain.Account.Commands.AccountGuides;
using Domain.Account.Commands.SubLeadgers.CashInBoxes;
using Domain.Account.InputModels.Subleadgers;
using Domain.Account.Models.Entities.AccountGuide;
using Domain.Account.Models.Entities.SubLeadgers;
using Domain.Account.Services.Interfaces;
using Shared;
using Shared.Responses;

namespace Domain.Account.Handlers.AccountGuides;

public class AccountGuideCreateCommandHandler(IAccountGuideService service): ICommandHandler<AccountGuideCreateCommand,ApiResponse<AccountGuide>>
{
    public async Task<ApiResponse<AccountGuide>> Handle(AccountGuideCreateCommand request,
        CancellationToken cancellationToken)
    {
        return  await service.Create(request);
    }
}