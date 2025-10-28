using Account.Domain.Commands.FinancialPeriods;
using Account.Domain.Models.Entities.FinancialPeriods;

namespace Account.Infrastructure.Handlers.FinancialPeriods;

public class FinancialPeriodCreateCommandHandler(IFinancialPeriodService service) : ICommandHandler<FinancialPeriodCreateCommand, ApiResponse<FinancialPeriod>>
{
    public async Task<ApiResponse<FinancialPeriod>> Handle(FinancialPeriodCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}
