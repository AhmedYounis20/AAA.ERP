using ERP.Application.Services.Account.SubLeadgers;
using ERP.Domain.Commands.Account.SubLeadgers.CashInBoxes;
using ERP.Domain.Models.Entities.Account.SubLeadgers;
using Shared;

namespace ERP.Infrastracture.Handlers.Account.CashInBoxes;

public class CashInBoxCreateCommandHandler(ICashInBoxService service) : ICommandHandler<CashInBoxCreateCommand, ApiResponse<CashInBox>>
{
    public async Task<ApiResponse<CashInBox>> Handle(CashInBoxCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}