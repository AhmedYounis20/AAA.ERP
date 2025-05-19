using Domain.Account.Commands.SubLeadgers.Banks;
using Domain.Account.Commands.SubLeadgers.CashInBoxes;
using Domain.Account.Commands.SubLeadgers.Suppliers;
using Domain.Account.Models.Entities.SubLeadgers;
using ERP.Application.Services.Account.SubLeadgers;
using Shared;
using Shared.Responses;

namespace ERP.Infrastracture.Handlers.Suppliers;

public class SupplierCreateCommandHandler(ISupplierService service): ICommandHandler<SupplierCreateCommand,ApiResponse<Supplier>>
{
    public async Task<ApiResponse<Supplier>> Handle(SupplierCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}