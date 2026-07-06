using Accounting.Application.Commands.FinancialPeriods;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.FinancialPeriods;

namespace Accounting.Infrastructure.Handlers.FinancialPeriods;

public sealed class FinancialPeriodUpdateCommandHandler(IFinancialPeriodWorkflowService workflow)
    : ICommandHandler<FinancialPeriodUpdateCommand, ApiResponse<FinancialPeriod>>
{
    public Task<ApiResponse<FinancialPeriod>> Handle(
        FinancialPeriodUpdateCommand request,
        CancellationToken cancellationToken)
        => workflow.Update(request);
}
