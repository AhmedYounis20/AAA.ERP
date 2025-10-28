using Account.Application.Services.SubLeadgers;
using Account.Domain.Commands.SubLeadgers.Customers;
using Account.Domain.Models.Entities.SubLeadgers;

namespace Account.Infrastructure.Handlers.Customers;

public class CustomerCreateCommandHandler(ICustomerService service) : ICommandHandler<CustomerCreateCommand, ApiResponse<Customer>>
{
    public async Task<ApiResponse<Customer>> Handle(CustomerCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}
