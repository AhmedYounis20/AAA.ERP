using Accounting.Application.Repositories;
using Accounting.Domain.Models.Entities.Roles;
using ERP.BuildingBlocks.Persistence.Repositories;

namespace Accounting.Infrastructure.Repositories;

public class RoleRepository : BaseSettingRepository<Role>, IRoleRepository
{
    public RoleRepository(IApplicationDbContext context) : base(context) { }
}
