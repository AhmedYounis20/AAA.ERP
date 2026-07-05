using Accounting.Application.Commands.FinancialPeriods;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.FinancialPeriods;

namespace Accounting.Infrastructure.Handlers.FinancialPeriods;

public class FinancialPeriodUpdateCommandHandler(IFinancialPeriodService service)
    : ICommandHandler<FinancialPeriodUpdateCommand, ApiResponse<FinancialPeriod>>
{
    public Task<ApiResponse<FinancialPeriod>> Handle(FinancialPeriodUpdateCommand request, CancellationToken cancellationToken)
        => service.Update(request);
}
