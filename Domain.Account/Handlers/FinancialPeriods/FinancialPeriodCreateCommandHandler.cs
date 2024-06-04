using AAA.ERP.Services.Interfaces.SubLeadgers;
using Domain.Account.Commands.FinancialPeriods;
using Domain.Account.InputModels.Subleadgers;
using Domain.Account.Models.Entities.FinancialPeriods;
using Shared;
using Shared.Responses;

namespace Domain.Account.Handlers.FinancialPeriods;

public class FinancialPeriodCreateCommandHandler(ICashInBoxService service): ICommandHandler<FinancialPeriodCreateCommand,ApiResponse<FinancialPeriod>>
{
    public async Task<ApiResponse<FinancialPeriod>> Handle(FinancialPeriodCreateCommand request,
        CancellationToken cancellationToken)
    {
        await service.Create(new BaseSubLeadgerInputModel());

        return new ApiResponse<FinancialPeriod>();
    }
}