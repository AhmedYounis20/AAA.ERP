using Domain.Account.Commands.ChartOfAccounts;
using Domain.Account.Models.Entities.ChartOfAccounts;
using ERP.Application.Services.Account;
using Shared;

namespace ERP.Infrastracture.Handlers.ChartOfAccounts;

public class ChartOfAccountCreateCommandHandler(IChartOfAccountService service) : ICommandHandler<ChartOfAccountCreateCommand, ApiResponse<ChartOfAccount>>
{
    public async Task<ApiResponse<ChartOfAccount>> Handle(ChartOfAccountCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}