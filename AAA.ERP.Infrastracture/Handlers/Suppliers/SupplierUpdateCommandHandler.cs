using Domain.Account.Commands.SubLeadgers.Banks;
using Domain.Account.Commands.SubLeadgers.Suppliers;
using Domain.Account.Models.Entities.SubLeadgers;
using ERP.Application.Services.Account.SubLeadgers;
using Shared;
using Shared.Responses;

namespace ERP.Infrastracture.Handlers.Suppliers;

public class SupplierUpdateCommandHandler(ISupplierService service): ICommandHandler<SupplierUpdateCommand,ApiResponse<Supplier>>
{
    public async Task<ApiResponse<Supplier>> Handle(SupplierUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}