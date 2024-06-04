using AAA.ERP.Services.Interfaces.SubLeadgers;
using Domain.Account.Commands.SubLeadgers.CashInBoxes;
using Domain.Account.InputModels.Subleadgers;
using Domain.Account.Models.Entities.SubLeadgers;
using Shared;
using Shared.Responses;

namespace Domain.Account.Handlers.CashInBoxes;

public class UpdateCashInBoxCommandHandlerCashInBoxCommandHandler(ICashInBoxService service): ICommandHandler<CashInBoxUpdateCommand,ApiResponse<CashInBox>>
{
    public async Task<ApiResponse<CashInBox>> Handle(CashInBoxUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Create(new BaseSubLeadgerInputModel());
    }
}