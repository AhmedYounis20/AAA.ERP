using AAA.ERP.Services.Interfaces.SubLeadgers;
using Domain.Account.Commands.ChartOfAccounts;
using Domain.Account.InputModels.Subleadgers;
using Domain.Account.Models.Entities.ChartOfAccounts;
using Domain.Account.Services.Interfaces;
using Shared;
using Shared.Responses;

namespace Domain.Account.Handlers.ChartOfAccounts;

public class ChartOfAccountCreateCommandHandler(IChartOfAccountService service): ICommandHandler<ChartOfAccountCreateCommand,ApiResponse<ChartOfAccount>>
{
    public async Task<ApiResponse<ChartOfAccount>> Handle(ChartOfAccountCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}