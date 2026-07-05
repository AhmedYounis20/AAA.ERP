using Accounting.Application.Commands.Suppliers;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.SubLeadgers;

namespace Accounting.Infrastructure.Handlers.Suppliers;

public class SupplierDeleteCommandHandler(ISupplierService supplierService)
    : ICommandHandler<SupplierDeleteCommand, ApiResponse<Supplier>>
{
    public Task<ApiResponse<Supplier>> Handle(SupplierDeleteCommand request, CancellationToken cancellationToken)
        => supplierService.Delete(request.Id);
}
