using ERP.Application.Services.Account.SubLeadgers;
using ERP.Domain.Commands.Account.SubLeadgers.CashInBoxes;
using ERP.Domain.Models.Entities.Account.SubLeadgers;
using Shared;

namespace ERP.Infrastracture.Handlers.Account.CashInBoxes;

public class CashInBoxUpdateCommandHandler(ICashInBoxService service) : ICommandHandler<CashInBoxUpdateCommand, ApiResponse<CashInBox>>
{
    public async Task<ApiResponse<CashInBox>> Handle(CashInBoxUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}