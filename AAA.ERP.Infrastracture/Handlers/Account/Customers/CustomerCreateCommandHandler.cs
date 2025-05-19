using ERP.Application.Services.Account.SubLeadgers;
using ERP.Domain.Commands.Account.SubLeadgers.Customers;
using ERP.Domain.Models.Entities.Account.SubLeadgers;

namespace ERP.Infrastracture.Handlers.Account.Customers;

public class CustomerCreateCommandHandler(ICustomerService service) : ICommandHandler<CustomerCreateCommand, ApiResponse<Customer>>
{
    public async Task<ApiResponse<Customer>> Handle(CustomerCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}