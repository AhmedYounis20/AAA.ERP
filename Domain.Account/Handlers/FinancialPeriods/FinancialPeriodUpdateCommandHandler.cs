using AAA.ERP.Services.Interfaces.SubLeadgers;
using Domain.Account.Commands.FinancialPeriods;
using Domain.Account.InputModels.Subleadgers;
using Domain.Account.Models.Entities.FinancialPeriods;
using Domain.Account.Services.Interfaces;
using Shared;
using Shared.Responses;

namespace Domain.Account.Handlers.FinancialPeriods;

public class FinancialPeriodUpdateCommandHandler(IFinancialPeriodService service): ICommandHandler<FinancialPeriodUpdateCommand,ApiResponse<FinancialPeriod>>
{
    public async Task<ApiResponse<FinancialPeriod>> Handle(FinancialPeriodUpdateCommand request, CancellationToken cancellationToken)
    {
         return await service.Update(request);
    }
}