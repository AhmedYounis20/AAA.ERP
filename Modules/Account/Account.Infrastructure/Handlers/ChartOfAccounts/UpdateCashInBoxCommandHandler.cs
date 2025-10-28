using Account.Domain.Commands.ChartOfAccounts;
using Account.Domain.Models.Entities.ChartOfAccounts;
using Shared;

namespace Account.Infrastructure.Handlers.ChartOfAccounts;

public class ChartOfAccountUpdateCommandHandler(IChartOfAccountService service) : ICommandHandler<ChartOfAccountUpdateCommand, ApiResponse<ChartOfAccount>>
{
    public async Task<ApiResponse<ChartOfAccount>> Handle(ChartOfAccountUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}
