using Accounting.Application.Commands.Customers;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.SubLeadgers;

namespace Accounting.Infrastructure.Handlers.Customers;

public class CustomerCreateCommandHandler(ICustomerService service)
    : ICommandHandler<CustomerCreateCommand, ApiResponse<Customer>>
{
    public Task<ApiResponse<Customer>> Handle(CustomerCreateCommand request, CancellationToken cancellationToken)
        => service.Create(request);
}
