using Domain.Account.Commands.SubLeadgers.Banks;
using Domain.Account.Commands.SubLeadgers.Customers;
using Domain.Account.Commands.SubLeadgers.Suppliers;
using Domain.Account.Models.Entities.SubLeadgers;
using ERP.Application.Services.Account.SubLeadgers;
using Shared;
using Shared.Responses;

namespace ERP.Infrastracture.Handlers.Customers;

public class CustomerUpdateCommandHandler(ICustomerService service): ICommandHandler<CustomerUpdateCommand,ApiResponse<Customer>>
{
    public async Task<ApiResponse<Customer>> Handle(CustomerUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}