using Domain.Account.Commands.SubLeadgers.Suppliers;
using Domain.Account.Models.Entities.SubLeadgers;

namespace ERP.Application.Services.Account.SubLeadgers;

public interface ISupplierService : IBaseSubLeadgerService<Supplier, SupplierCreateCommand, SupplierUpdateCommand>
{ }