using ERP.Application.Repositories.Account;
using ERP.Domain.Models.Entities.Account.Roles;
using ERP.Infrastracture.Repositories.BaseRepositories;

namespace ERP.Infrastracture.Repositories.Account;

public class RoleRepository : BaseSettingRepository<Role>, IRoleRepository
{
    public RoleRepository(ApplicationDbContext context) : base(context) {}
}
