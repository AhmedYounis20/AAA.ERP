using Accounting.Application.Commands.Customers;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.SubLeadgers;

namespace Accounting.Infrastructure.Handlers.Customers;

public class CustomerUpdateCommandHandler(ICustomerService service)
    : ICommandHandler<CustomerUpdateCommand, ApiResponse<Customer>>
{
    public Task<ApiResponse<Customer>> Handle(CustomerUpdateCommand request, CancellationToken cancellationToken)
        => service.Update(request);
}
