using Accounting.Application.Commands.FinancialPeriods;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.FinancialPeriods;

namespace Accounting.Infrastructure.Handlers.FinancialPeriods;

public sealed class FinancialPeriodCreateCommandHandler(IFinancialPeriodWorkflowService workflow)
    : ICommandHandler<FinancialPeriodCreateCommand, ApiResponse<FinancialPeriod>>
{
    public Task<ApiResponse<FinancialPeriod>> Handle(
        FinancialPeriodCreateCommand request,
        CancellationToken cancellationToken)
        => workflow.Create(request);
}
