using Accounting.Application.Repositories.Account.SubLeadgers;
using Accounting.Domain.Models.Entities.SubLeadgers;

namespace Accounting.Application.Repositories.Account.SubLeadgers;

/// <summary>
/// Repository contract for supplier persistence.
/// </summary>
public interface ISupplierRepository : IBaseSubLeadgerRepository<Supplier>
{ }
