using ERP.Domain.Commands.Account.SubLeadgers.Customers;
using ERP.Domain.Models.Entities.Account.SubLeadgers;

namespace ERP.Application.Services.Account.SubLeadgers;

public interface ICustomerService : IBaseSubLeadgerService<Customer, CustomerCreateCommand, CustomerUpdateCommand>
{ }