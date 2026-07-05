using Accounting.Application.Repositories.Account.SubLeadgers;
using Accounting.Domain.Models.Entities.SubLeadgers;

namespace Accounting.Application.Repositories;

/// <summary>
/// Repository contract for customer persistence.
/// </summary>
public interface ICustomerRepository : IBaseSubLeadgerRepository<Customer>;
