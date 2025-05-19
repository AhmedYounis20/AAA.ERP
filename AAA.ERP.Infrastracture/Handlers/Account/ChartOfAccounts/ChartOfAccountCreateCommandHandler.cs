using ERP.Application.Services.Account;
using ERP.Domain.Commands.Account.ChartOfAccounts;
using ERP.Domain.Models.Entities.Account.ChartOfAccounts;
using Shared;

namespace ERP.Infrastracture.Handlers.Account.ChartOfAccounts;

public class ChartOfAccountCreateCommandHandler(IChartOfAccountService service) : ICommandHandler<ChartOfAccountCreateCommand, ApiResponse<ChartOfAccount>>
{
    public async Task<ApiResponse<ChartOfAccount>> Handle(ChartOfAccountCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}