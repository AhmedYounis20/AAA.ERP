using Accounting.Application.Commands.Suppliers;
using Accounting.Application.Services.SubLeadgers;
using Accounting.Domain.Models.Entities.SubLeadgers;

namespace Accounting.Application.Services;

/// <summary>
/// Application service contract for supplier.
/// </summary>
public interface ISupplierService : IBaseSubLeadgerService<Supplier, SupplierCreateCommand, SupplierUpdateCommand>;
