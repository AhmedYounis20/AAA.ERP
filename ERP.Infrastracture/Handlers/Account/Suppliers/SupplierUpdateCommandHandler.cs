using ERP.Application.Services.Account.SubLeadgers;
using ERP.Domain.Commands.Account.SubLeadgers.Suppliers;
using ERP.Domain.Models.Entities.Account.SubLeadgers;

namespace ERP.Infrastracture.Handlers.Account.Suppliers;

public class SupplierUpdateCommandHandler(ISupplierService service) : ICommandHandler<SupplierUpdateCommand, ApiResponse<Supplier>>
{
    public async Task<ApiResponse<Supplier>> Handle(SupplierUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}