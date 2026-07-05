using Accounting.Application.Commands.ChartOfAccounts;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.ChartOfAccounts;

namespace Accounting.Infrastructure.Handlers.ChartOfAccounts;

public class ChartOfAccountDeleteCommandHandler(IChartOfAccountService chartOfAccountService)
    : ICommandHandler<ChartOfAccountDeleteCommand, ApiResponse<ChartOfAccount>>
{
    public Task<ApiResponse<ChartOfAccount>> Handle(ChartOfAccountDeleteCommand request, CancellationToken cancellationToken)
        => chartOfAccountService.Delete(request.Id);
}
