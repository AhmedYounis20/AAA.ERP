using ERP.Domain.Commands.Account.FinancialPeriods;
using ERP.Domain.Models.Entities.Account.FinancialPeriods;

namespace ERP.Infrastracture.Handlers.Account.FinancialPeriods;

public class FinancialPeriodUpdateCommandHandler(IFinancialPeriodService service) : ICommandHandler<FinancialPeriodUpdateCommand, ApiResponse<FinancialPeriod>>
{
    public async Task<ApiResponse<FinancialPeriod>> Handle(FinancialPeriodUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}