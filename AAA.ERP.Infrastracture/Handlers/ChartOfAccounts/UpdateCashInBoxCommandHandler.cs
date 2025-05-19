using Domain.Account.Commands.ChartOfAccounts;
using Domain.Account.Models.Entities.ChartOfAccounts;
using ERP.Application.Services.Account;
using Shared;

namespace ERP.Infrastracture.Handlers.ChartOfAccounts;

public class ChartOfAccountUpdateCommandHandler(IChartOfAccountService service) : ICommandHandler<ChartOfAccountUpdateCommand, ApiResponse<ChartOfAccount>>
{
    public async Task<ApiResponse<ChartOfAccount>> Handle(ChartOfAccountUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}