using AAA.ERP.Services.Interfaces.SubLeadgers;
using Domain.Account.Commands.FinancialPeriods;
using Domain.Account.InputModels.Subleadgers;
using Domain.Account.Models.Entities.FinancialPeriods;
using Domain.Account.Services.Interfaces;
using Shared;
using Shared.Responses;

namespace Domain.Account.Handlers.FinancialPeriods;

public class FinancialPeriodCreateCommandHandler(IFinancialPeriodService service): ICommandHandler<FinancialPeriodCreateCommand,ApiResponse<FinancialPeriod>>
{
    public async Task<ApiResponse<FinancialPeriod>> Handle(FinancialPeriodCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}