using Accounting.Application.Commands.FinancialPeriods;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.FinancialPeriods;

namespace Accounting.Infrastructure.Handlers.FinancialPeriods;

public class FinancialPeriodDeleteCommandHandler(IFinancialPeriodService financialPeriodService)
    : ICommandHandler<FinancialPeriodDeleteCommand, ApiResponse<FinancialPeriod>>
{
    public Task<ApiResponse<FinancialPeriod>> Handle(FinancialPeriodDeleteCommand request, CancellationToken cancellationToken)
        => financialPeriodService.Delete(request.Id);
}
