using ERP.Application.Services.Account;
using ERP.Domain.Commands.Account.FinancialPeriods;
using ERP.Domain.Models.Entities.Account.FinancialPeriods;
using Shared;

namespace ERP.Infrastracture.Handlers.Account.FinancialPeriods;

public class FinancialPeriodCreateCommandHandler(IFinancialPeriodService service) : ICommandHandler<FinancialPeriodCreateCommand, ApiResponse<FinancialPeriod>>
{
    public async Task<ApiResponse<FinancialPeriod>> Handle(FinancialPeriodCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}