using ERP.Application.Services.Account;
using ERP.Domain.Commands.Account.ChartOfAccounts;
using ERP.Domain.Models.Entities.Account.ChartOfAccounts;
using Shared;

namespace ERP.Infrastracture.Handlers.Account.ChartOfAccounts;

public class ChartOfAccountUpdateCommandHandler(IChartOfAccountService service) : ICommandHandler<ChartOfAccountUpdateCommand, ApiResponse<ChartOfAccount>>
{
    public async Task<ApiResponse<ChartOfAccount>> Handle(ChartOfAccountUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}