using Account.Application.Repositories;
using Account.Domain.Models.Entities.Roles;
using Shared.Infrastructure.Repositories;

namespace Account.Infrastructure.Repositories;

public class RoleRepository : BaseSettingRepository<Role>, IRoleRepository
{
    public RoleRepository(IAccountDbContext context) : base(context) {}
}
