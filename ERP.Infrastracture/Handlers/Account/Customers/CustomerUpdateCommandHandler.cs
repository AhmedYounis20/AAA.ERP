using ERP.Application.Services.Account.SubLeadgers;
using ERP.Domain.Commands.Account.SubLeadgers.Customers;
using ERP.Domain.Models.Entities.Account.SubLeadgers;

namespace ERP.Infrastracture.Handlers.Account.Customers;

public class CustomerUpdateCommandHandler(ICustomerService service) : ICommandHandler<CustomerUpdateCommand, ApiResponse<Customer>>
{
    public async Task<ApiResponse<Customer>> Handle(CustomerUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}