using AAA.ERP.Services.Interfaces.SubLeadgers;
using Domain.Account.Commands.ChartOfAccounts;
using Domain.Account.Commands.SubLeadgers.CashInBoxes;
using Domain.Account.InputModels.Subleadgers;
using Domain.Account.Models.Entities.ChartOfAccounts;
using Domain.Account.Models.Entities.SubLeadgers;
using Shared;
using Shared.Responses;

namespace Domain.Account.Handlers.CashInBoxes;

public class ChartOfAccountUpdateCommandHandler(ICashInBoxService service): ICommandHandler<ChartOfAccountUpdateCommand,ApiResponse<ChartOfAccount>>
{
    public async Task<ApiResponse<ChartOfAccount>> Handle(ChartOfAccountUpdateCommand request, CancellationToken cancellationToken)
    {
         await service.Create(new BaseSubLeadgerInputModel());

         return new ApiResponse<ChartOfAccount>();
    }
}