using AAA.ERP.Services.Interfaces.SubLeadgers;
using Domain.Account.Commands.SubLeadgers.Banks;
using Domain.Account.Commands.SubLeadgers.Customers;
using Domain.Account.Commands.SubLeadgers.Suppliers;
using Domain.Account.Models.Entities.SubLeadgers;

namespace Domain.Account.Services.Interfaces.SubLeadgers;

public interface ICustomerService : IBaseSubLeadgerService<Customer,CustomerCreateCommand,CustomerUpdateCommand>
{ }