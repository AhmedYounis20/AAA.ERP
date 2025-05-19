using Domain.Account.Commands.Roles;
using Domain.Account.Models.Entities.Roles;
using ERP.Application.Services.Account;

namespace ERP.Infrastracture.Services.Account;
public class RoleService :
    BaseSettingService<Role, RoleCreateCommand, RoleUpdateCommand>, IRoleService
{
    public RoleService(IRoleRepository repository) : base(repository)
    { }
}