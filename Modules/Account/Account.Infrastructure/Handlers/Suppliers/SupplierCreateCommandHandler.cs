using Account.Application.Services.SubLeadgers;
using Account.Domain.Commands.SubLeadgers.Suppliers;
using Account.Domain.Models.Entities.SubLeadgers;

namespace Account.Infrastructure.Handlers.Suppliers;

public class SupplierCreateCommandHandler(ISupplierService service) : ICommandHandler<SupplierCreateCommand, ApiResponse<Supplier>>
{
    public async Task<ApiResponse<Supplier>> Handle(SupplierCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}
