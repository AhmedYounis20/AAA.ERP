using Account.Domain.Commands.FinancialPeriods;
using Account.Domain.Models.Entities.FinancialPeriods;

namespace Account.Infrastructure.Handlers.FinancialPeriods;

public class FinancialPeriodUpdateCommandHandler(IFinancialPeriodService service) : ICommandHandler<FinancialPeriodUpdateCommand, ApiResponse<FinancialPeriod>>
{
    public async Task<ApiResponse<FinancialPeriod>> Handle(FinancialPeriodUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}
