using Account.Application.Repositories;
using Account.Domain.Commands.Roles;
using Account.Domain.Models.Entities.Roles;

namespace Account.Infrastructure.Services;
public class RoleService :
    BaseSettingService<Role, RoleCreateCommand, RoleUpdateCommand>, IRoleService
{
    public RoleService(IRoleRepository repository) : base(repository)
    { }
}
