using Accounting.Application.Commands.Roles;
using Accounting.Application.Repositories;
using Accounting.Application.Services;
using ERP.Domain.Models.Entities.Account.Roles;
using ERP.Infrastructure.Services.BaseServices;

namespace Accounting.Infrastructure.Services;

public class RoleService
    : BaseSettingService<Role, RoleCreateCommand, RoleUpdateCommand>, IRoleService
{
    public RoleService(IRoleRepository repository) : base(repository) { }
}
