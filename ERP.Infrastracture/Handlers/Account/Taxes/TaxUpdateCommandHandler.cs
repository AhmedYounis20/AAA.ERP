using ERP.Application.Services.Account.SubLeadgers;
using ERP.Domain.Commands.Account.SubLeadgers.Taxes;
using ERP.Domain.Models.Entities.Account.SubLeadgers;

namespace ERP.Infrastracture.Handlers.Account.Taxes;

public class TaxUpdateCommandHandler(ITaxService service) : ICommandHandler<TaxUpdateCommand, ApiResponse<Tax>>
{
    public async Task<ApiResponse<Tax>> Handle(TaxUpdateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}
