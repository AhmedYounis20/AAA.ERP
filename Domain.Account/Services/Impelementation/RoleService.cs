using Domain.Account.Commands.Roles;
using Domain.Account.Models.Entities.Roles;
using Domain.Account.Repositories.Interfaces;
using Domain.Account.Services.BaseServices.impelemtation;
using Domain.Account.Services.Interfaces;

namespace AAA.ERP.Services.Impelementation;

using AAA.ERP.Services.Interfaces;

public class RoleService : 
    BaseSettingService<Role,RoleCreateCommand,RoleUpdateCommand>, IRoleService
{
    public RoleService(IRoleRepository repository) : base(repository)
    { }
}