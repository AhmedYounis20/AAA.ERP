using Account.Application.Services.SubLeadgers;
using Account.Domain.Commands.SubLeadgers.Customers;
using Account.Domain.Models.Entities.SubLeadgers;

namespace Account.Infrastructure.Handlers.Customers;

public class CustomerUpdateCommandHandler(ICustomerService service) : ICommandHandler<CustomerUpdateCommand, ApiResponse<Customer>>
{
    public async Task<ApiResponse<Customer>> Handle(CustomerUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}
