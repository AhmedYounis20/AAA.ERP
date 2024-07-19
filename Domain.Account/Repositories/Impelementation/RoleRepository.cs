using Domain.Account.DBConfiguration.DbContext;
using Domain.Account.Models.Entities.AccountGuide;
using Domain.Account.Models.Entities.Roles;
using Domain.Account.Repositories.BaseRepositories.Impelementation;
using Domain.Account.Repositories.Interfaces;

namespace Domain.Account.Repositories.Impelementation;

public class RoleRepository : BaseSettingRepository<Role>, IRoleRepository
{
    public RoleRepository(ApplicationDbContext context) : base(context) {}
}
