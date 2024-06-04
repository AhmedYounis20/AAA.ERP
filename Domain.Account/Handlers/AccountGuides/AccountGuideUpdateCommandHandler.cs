using AAA.ERP.Services.Interfaces.SubLeadgers;
using Domain.Account.Commands.AccountGuides;
using Domain.Account.InputModels.Subleadgers;
using Domain.Account.Models.Entities.AccountGuide;
using Shared;
using Shared.Responses;

namespace Domain.Account.Handlers.AccountGuides;

public class AccountGuideUpdateCommandHandler(ICashInBoxService service): ICommandHandler<AccountGuideUpdateCommand,ApiResponse<AccountGuide>>
{
    public async Task<ApiResponse<AccountGuide>> Handle(AccountGuideUpdateCommand request, CancellationToken cancellationToken)
    {
        await service.Create(new BaseSubLeadgerInputModel());
        return new ApiResponse<AccountGuide>();
    }
}