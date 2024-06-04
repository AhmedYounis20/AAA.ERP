using AAA.ERP.Services.Interfaces.SubLeadgers;
using Domain.Account.Commands.FinancialPeriods;
using Domain.Account.InputModels.Subleadgers;
using Domain.Account.Models.Entities.FinancialPeriods;
using Shared;
using Shared.Responses;

namespace Domain.Account.Handlers.FinancialPeriods;

public class FinancialPeriodUpdateCommandHandler(ICashInBoxService service): ICommandHandler<FinancialPeriodUpdateCommand,ApiResponse<FinancialPeriod>>
{
    public async Task<ApiResponse<FinancialPeriod>> Handle(FinancialPeriodUpdateCommand request, CancellationToken cancellationToken)
    {
         await service.Create(new BaseSubLeadgerInputModel());
         return new ApiResponse<FinancialPeriod>();
    }
}