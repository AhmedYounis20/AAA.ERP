using Accounting.Application.Repositories;
using ERP.Domain.Models.Entities.Account.Roles;
using ERP.Infrastracture.Repositories.BaseRepositories;

namespace Accounting.Infrastructure.Repositories;

public class RoleRepository : BaseSettingRepository<Role>, IRoleRepository
{
    public RoleRepository(IApplicationDbContext context) : base(context) { }
}
