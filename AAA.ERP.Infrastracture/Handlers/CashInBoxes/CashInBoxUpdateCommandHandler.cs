using Domain.Account.Commands.SubLeadgers.CashInBoxes;
using Domain.Account.Models.Entities.SubLeadgers;
using ERP.Application.Services.Account.SubLeadgers;
using Shared;

namespace ERP.Infrastracture.Handlers.CashInBoxes;

public class CashInBoxUpdateCommandHandler(ICashInBoxService service) : ICommandHandler<CashInBoxUpdateCommand, ApiResponse<CashInBox>>
{
    public async Task<ApiResponse<CashInBox>> Handle(CashInBoxUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}