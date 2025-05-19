using ERP.Domain.Commands.Account.SubLeadgers.Suppliers;
using ERP.Domain.Models.Entities.Account.SubLeadgers;

namespace ERP.Application.Services.Account.SubLeadgers;

public interface ISupplierService : IBaseSubLeadgerService<Supplier, SupplierCreateCommand, SupplierUpdateCommand>
{ }