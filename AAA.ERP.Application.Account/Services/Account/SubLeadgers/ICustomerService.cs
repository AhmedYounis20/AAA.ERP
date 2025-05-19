using Domain.Account.Commands.SubLeadgers.Customers;
using Domain.Account.Models.Entities.SubLeadgers;

namespace ERP.Application.Services.Account.SubLeadgers;

public interface ICustomerService : IBaseSubLeadgerService<Customer, CustomerCreateCommand, CustomerUpdateCommand>
{ }