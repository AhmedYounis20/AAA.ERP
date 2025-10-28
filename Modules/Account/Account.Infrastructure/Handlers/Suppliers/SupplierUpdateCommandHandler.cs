using Account.Application.Services.SubLeadgers;
using Account.Domain.Commands.SubLeadgers.Suppliers;
using Account.Domain.Models.Entities.SubLeadgers;

namespace Account.Infrastructure.Handlers.Suppliers;

public class SupplierUpdateCommandHandler(ISupplierService service) : ICommandHandler<SupplierUpdateCommand, ApiResponse<Supplier>>
{
    public async Task<ApiResponse<Supplier>> Handle(SupplierUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}
