using Domain.Account.DBConfiguration.DbContext;
using Domain.Account.Models.Entities.SubLeadgers;
using Domain.Account.Repositories.BaseRepositories.Impelementation;
using Domain.Account.Repositories.Interfaces.SubLeadgers;
using Shared.BaseRepositories.Impelementation;

namespace Domain.Account.Repositories.Impelementation.SubLeadgers;

public class SupplierRepository : BaseSubLeadgerRepository<Supplier>, ISupplierRepository
{
    public SupplierRepository(ApplicationDbContext context) : base(context){}
}