using Domain.Account.Commands.SubLeadgers.Banks;
using Domain.Account.Commands.SubLeadgers.CashInBoxes;
using Domain.Account.Commands.SubLeadgers.Customers;
using Domain.Account.Commands.SubLeadgers.Suppliers;
using Domain.Account.Models.Entities.SubLeadgers;
using Domain.Account.Services.Interfaces.SubLeadgers;
using Shared;
using Shared.Responses;

namespace Domain.Account.Handlers.Customers;

public class CustomerCreateCommandHandler(ICustomerService service): ICommandHandler<CustomerCreateCommand,ApiResponse<Customer>>
{
    public async Task<ApiResponse<Customer>> Handle(CustomerCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}