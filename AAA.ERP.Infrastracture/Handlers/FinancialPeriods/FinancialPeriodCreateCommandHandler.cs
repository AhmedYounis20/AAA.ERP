using Domain.Account.Commands.FinancialPeriods;
using Domain.Account.Models.Entities.FinancialPeriods;
using ERP.Application.Services.Account;
using Shared;

namespace ERP.Infrastracture.Handlers.FinancialPeriods;

public class FinancialPeriodCreateCommandHandler(IFinancialPeriodService service): ICommandHandler<FinancialPeriodCreateCommand,ApiResponse<FinancialPeriod>>
{
    public async Task<ApiResponse<FinancialPeriod>> Handle(FinancialPeriodCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}