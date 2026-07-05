using Accounting.Application.Commands.ChartOfAccounts;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.ChartOfAccounts;

namespace Accounting.Infrastructure.Handlers.ChartOfAccounts;

public class ChartOfAccountCreateCommandHandler(IChartOfAccountService service)
    : ICommandHandler<ChartOfAccountCreateCommand, ApiResponse<ChartOfAccount>>
{
    public Task<ApiResponse<ChartOfAccount>> Handle(ChartOfAccountCreateCommand request, CancellationToken cancellationToken)
        => service.Create(request);
}
