using Account.Domain.Commands.ChartOfAccounts;
using Account.Domain.Models.Entities.ChartOfAccounts;
using Shared;

namespace Account.Infrastructure.Handlers.ChartOfAccounts;

public class ChartOfAccountCreateCommandHandler(IChartOfAccountService service) : ICommandHandler<ChartOfAccountCreateCommand, ApiResponse<ChartOfAccount>>
{
    public async Task<ApiResponse<ChartOfAccount>> Handle(ChartOfAccountCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}
