using Accounting.Application.Commands.Suppliers;
using Accounting.Application.Services;
using ERP.Domain.Models.Entities.Account.SubLeadgers;

namespace Accounting.Infrastructure.Handlers.Suppliers;

public class SupplierCreateCommandHandler(ISupplierService service)
    : ICommandHandler<SupplierCreateCommand, ApiResponse<Supplier>>
{
    public Task<ApiResponse<Supplier>> Handle(SupplierCreateCommand request, CancellationToken cancellationToken)
        => service.Create(request);
}
