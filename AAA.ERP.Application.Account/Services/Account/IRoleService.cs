using Domain.Account.Commands.Roles;
using Domain.Account.Models.Entities.Roles;
using ERP.Application.Services.BaseServices;

namespace ERP.Application.Services.Account;

public interface IRoleService: IBaseSettingService<Role,RoleCreateCommand,RoleUpdateCommand>{}