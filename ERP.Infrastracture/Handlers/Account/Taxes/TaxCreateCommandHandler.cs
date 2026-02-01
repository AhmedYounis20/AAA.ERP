using ERP.Application.Services.Account.SubLeadgers;
using ERP.Domain.Commands.Account.SubLeadgers.Taxes;
using ERP.Domain.Models.Entities.Account.SubLeadgers;

namespace ERP.Infrastracture.Handlers.Account.Taxes;

public class TaxCreateCommandHandler(ITaxService service) : ICommandHandler<TaxCreateCommand, ApiResponse<Tax>>
{
    public async Task<ApiResponse<Tax>> Handle(TaxCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}
