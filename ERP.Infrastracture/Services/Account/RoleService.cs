using ERP.Application.Repositories.Account;
using ERP.Application.Services.Account;
using ERP.Domain.Commands.Account.Roles;
using ERP.Domain.Models.Entities.Account.Roles;

namespace ERP.Infrastracture.Services.Account;
public class RoleService :
    BaseSettingService<Role, RoleCreateCommand, RoleUpdateCommand>, IRoleService
{
    public RoleService(IRoleRepository repository) : base(repository)
    { }
}