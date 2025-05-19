using ERP.Application.Services.Account.SubLeadgers;
using ERP.Domain.Commands.Account.SubLeadgers.Suppliers;
using ERP.Domain.Models.Entities.Account.SubLeadgers;

namespace ERP.Infrastracture.Handlers.Account.Suppliers;

public class SupplierCreateCommandHandler(ISupplierService service) : ICommandHandler<SupplierCreateCommand, ApiResponse<Supplier>>
{
    public async Task<ApiResponse<Supplier>> Handle(SupplierCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}