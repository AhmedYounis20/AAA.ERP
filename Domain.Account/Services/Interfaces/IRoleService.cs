using Domain.Account.Commands.Roles;
using Domain.Account.Models.Entities.Roles;
using Domain.Account.Services.BaseServices.interfaces;

namespace Domain.Account.Services.Interfaces;

public interface IRoleService: IBaseSettingService<Role,RoleCreateCommand,RoleUpdateCommand>{}