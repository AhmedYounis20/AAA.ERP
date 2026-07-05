using Accounting.Application.Commands.Customers;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.SubLeadgers;

namespace Accounting.Infrastructure.Handlers.Customers;

public class CustomerDeleteCommandHandler(ICustomerService customerService)
    : ICommandHandler<CustomerDeleteCommand, ApiResponse<Customer>>
{
    public Task<ApiResponse<Customer>> Handle(CustomerDeleteCommand request, CancellationToken cancellationToken)
        => customerService.Delete(request.Id);
}
