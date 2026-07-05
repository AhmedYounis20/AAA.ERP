using Accounting.Application.Commands.ChartOfAccounts;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.ChartOfAccounts;

namespace Accounting.Infrastructure.Handlers.ChartOfAccounts;

public class ChartOfAccountUpdateCommandHandler(IChartOfAccountService service)
    : ICommandHandler<ChartOfAccountUpdateCommand, ApiResponse<ChartOfAccount>>
{
    public Task<ApiResponse<ChartOfAccount>> Handle(ChartOfAccountUpdateCommand request, CancellationToken cancellationToken)
        => service.Update(request);
}
