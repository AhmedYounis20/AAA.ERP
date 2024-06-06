using AAA.ERP.Services.Interfaces.SubLeadgers;
using Domain.Account.Commands.ChartOfAccounts;
using Domain.Account.Commands.SubLeadgers.CashInBoxes;
using Domain.Account.InputModels.Subleadgers;
using Domain.Account.Models.Entities.ChartOfAccounts;
using Domain.Account.Models.Entities.SubLeadgers;
using Domain.Account.Services.Interfaces;
using Shared;
using Shared.Responses;

namespace Domain.Account.Handlers.CashInBoxes;

public class ChartOfAccountUpdateCommandHandler(IChartOfAccountService service): ICommandHandler<ChartOfAccountUpdateCommand,ApiResponse<ChartOfAccount>>
{
    public async Task<ApiResponse<ChartOfAccount>> Handle(ChartOfAccountUpdateCommand request, CancellationToken cancellationToken)
    {
         return await service.Update(request);
    }
}