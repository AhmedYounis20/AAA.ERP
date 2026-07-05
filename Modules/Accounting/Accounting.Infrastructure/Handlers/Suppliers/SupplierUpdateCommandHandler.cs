using Accounting.Application.Commands.Suppliers;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.SubLeadgers;

namespace Accounting.Infrastructure.Handlers.Suppliers;

public class SupplierUpdateCommandHandler(ISupplierService service)
    : ICommandHandler<SupplierUpdateCommand, ApiResponse<Supplier>>
{
    public Task<ApiResponse<Supplier>> Handle(SupplierUpdateCommand request, CancellationToken cancellationToken)
        => service.Update(request);
}
