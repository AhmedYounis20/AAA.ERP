using AAA.ERP.Services.Interfaces.SubLeadgers;
using Domain.Account.Commands.ChartOfAccounts;
using Domain.Account.InputModels.Subleadgers;
using Domain.Account.Models.Entities.ChartOfAccounts;
using Shared;
using Shared.Responses;

namespace Domain.Account.Handlers.ChartOfAccounts;

public class ChartOfAccountCreateCommandHandler(ICashInBoxService service): ICommandHandler<ChartOfAccountCreateCommand,ApiResponse<ChartOfAccount>>
{
    public async Task<ApiResponse<ChartOfAccount>> Handle(ChartOfAccountCreateCommand request,
        CancellationToken cancellationToken)
    {
        await service.Create(new BaseSubLeadgerInputModel());
        return new ApiResponse<ChartOfAccount>();
    }
}