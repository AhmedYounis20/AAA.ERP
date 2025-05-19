using Domain.Account.Commands.FinancialPeriods;
using Domain.Account.Models.Entities.FinancialPeriods;

namespace ERP.Infrastracture.Handlers.FinancialPeriods;

public class FinancialPeriodUpdateCommandHandler(IFinancialPeriodService service): ICommandHandler<FinancialPeriodUpdateCommand,ApiResponse<FinancialPeriod>>
{
    public async Task<ApiResponse<FinancialPeriod>> Handle(FinancialPeriodUpdateCommand request, CancellationToken cancellationToken)
    {
         return await service.Update(request);
    }
}