using Account.Domain.Commands.SubLeadgers.Suppliers;
using Account.Domain.Models.Entities.SubLeadgers;

namespace Account.Application.Services.SubLeadgers;

public interface ISupplierService : IBaseSubLeadgerService<Supplier, SupplierCreateCommand, SupplierUpdateCommand>
{ }