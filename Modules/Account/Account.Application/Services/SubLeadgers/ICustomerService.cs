using Account.Domain.Commands.SubLeadgers.Customers;
using Account.Domain.Models.Entities.SubLeadgers;

namespace Account.Application.Services.SubLeadgers;

public interface ICustomerService : IBaseSubLeadgerService<Customer, CustomerCreateCommand, CustomerUpdateCommand>
{ }