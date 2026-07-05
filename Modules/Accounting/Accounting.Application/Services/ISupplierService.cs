using Accounting.Application.Commands.Suppliers;
using Accounting.Application.Services.SubLeadgers;
using Accounting.Domain.Models.Entities.SubLeadgers;

namespace Accounting.Application.Services;

public interface ISupplierService : IBaseSubLeadgerService<Supplier, SupplierCreateCommand, SupplierUpdateCommand>;
