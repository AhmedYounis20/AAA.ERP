using Accounting.Application.Commands.FinancialPeriods;
using Accounting.Application.Services;
using ERP.Domain.Models.Entities.Account.FinancialPeriods;

namespace Accounting.Infrastructure.Handlers.FinancialPeriods;

public class FinancialPeriodCreateCommandHandler(IFinancialPeriodService service)
    : ICommandHandler<FinancialPeriodCreateCommand, ApiResponse<FinancialPeriod>>
{
    public Task<ApiResponse<FinancialPeriod>> Handle(FinancialPeriodCreateCommand request, CancellationToken cancellationToken)
        => service.Create(request);
}
