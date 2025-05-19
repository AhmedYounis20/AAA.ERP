using Domain.Account.Models.Entities.Roles;
using ERP.Infrastracture.Repositories.BaseRepositories;

namespace ERP.Infrastracture.Repositories.Account;

public class RoleRepository : BaseSettingRepository<Role>, IRoleRepository
{
    public RoleRepository(ApplicationDbContext context) : base(context) {}
}
