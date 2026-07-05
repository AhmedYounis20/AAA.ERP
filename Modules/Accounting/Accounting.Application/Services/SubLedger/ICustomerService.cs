using Accounting.Application.Commands.Customers;
using Accounting.Application.Services.SubLeadgers;
using Accounting.Domain.Models.Entities.SubLeadgers;

namespace Accounting.Application.Services;

/// <summary>
/// Application service contract for customer.
/// </summary>
public interface ICustomerService : IBaseSubLeadgerService<Customer, CustomerCreateCommand, CustomerUpdateCommand>;
