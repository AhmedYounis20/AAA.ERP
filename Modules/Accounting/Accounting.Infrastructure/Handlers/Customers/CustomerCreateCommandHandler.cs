using Accounting.Application.Commands.Customers;
using Accounting.Application.Services;
using ERP.Domain.Models.Entities.Account.SubLeadgers;

namespace Accounting.Infrastructure.Handlers.Customers;

public class CustomerCreateCommandHandler(ICustomerService service)
    : ICommandHandler<CustomerCreateCommand, ApiResponse<Customer>>
{
    public Task<ApiResponse<Customer>> Handle(CustomerCreateCommand request, CancellationToken cancellationToken)
        => service.Create(request);
}
