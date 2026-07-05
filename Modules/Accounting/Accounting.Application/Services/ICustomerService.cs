using Accounting.Application.Commands.Customers;
using Accounting.Application.Services.SubLeadgers;
using Accounting.Domain.Models.Entities.SubLeadgers;

namespace Accounting.Application.Services;

public interface ICustomerService : IBaseSubLeadgerService<Customer, CustomerCreateCommand, CustomerUpdateCommand>;
