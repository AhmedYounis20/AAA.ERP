using Domain.Account.Commands.SubLeadgers.CashInBoxes;
using Domain.Account.Models.Entities.SubLeadgers;
using ERP.Application.Services.Account.SubLeadgers;
using Shared;

namespace ERP.Infrastracture.Handlers.CashInBoxes;

public class CashInBoxCreateCommandHandler(ICashInBoxService service) : ICommandHandler<CashInBoxCreateCommand, ApiResponse<CashInBox>>
{
    public async Task<ApiResponse<CashInBox>> Handle(CashInBoxCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}