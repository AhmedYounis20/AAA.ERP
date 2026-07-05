using Accounting.Application.Commands.ChartOfAccounts;
using Accounting.Application.Services;
using ERP.Domain.Models.Entities.Account.ChartOfAccounts;

namespace Accounting.Infrastructure.Handlers.ChartOfAccounts;

public class ChartOfAccountUpdateCommandHandler(IChartOfAccountService service)
    : ICommandHandler<ChartOfAccountUpdateCommand, ApiResponse<ChartOfAccount>>
{
    public Task<ApiResponse<ChartOfAccount>> Handle(ChartOfAccountUpdateCommand request, CancellationToken cancellationToken)
        => service.Update(request);
}
