using Accounting.Application.Commands.FinancialPeriods;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.FinancialPeriods;

namespace Accounting.Infrastructure.Handlers.FinancialPeriods;

public sealed class FinancialPeriodDeleteCommandHandler(IFinancialPeriodWorkflowService workflow)
    : ICommandHandler<FinancialPeriodDeleteCommand, ApiResponse<FinancialPeriod>>
{
    public Task<ApiResponse<FinancialPeriod>> Handle(
        FinancialPeriodDeleteCommand request,
        CancellationToken cancellationToken)
        => workflow.Delete(request.Id);
}
