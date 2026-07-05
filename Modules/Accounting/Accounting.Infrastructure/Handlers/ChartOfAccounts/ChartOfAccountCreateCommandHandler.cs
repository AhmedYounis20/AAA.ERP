using Accounting.Application.Commands.ChartOfAccounts;
using Accounting.Application.Services;
using ERP.Domain.Models.Entities.Account.ChartOfAccounts;

namespace Accounting.Infrastructure.Handlers.ChartOfAccounts;

public class ChartOfAccountCreateCommandHandler(IChartOfAccountService service)
    : ICommandHandler<ChartOfAccountCreateCommand, ApiResponse<ChartOfAccount>>
{
    public Task<ApiResponse<ChartOfAccount>> Handle(ChartOfAccountCreateCommand request, CancellationToken cancellationToken)
        => service.Create(request);
}
