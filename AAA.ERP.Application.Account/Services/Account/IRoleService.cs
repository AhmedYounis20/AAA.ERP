using ERP.Application.Services.BaseServices;
using ERP.Domain.Commands.Account.Roles;
using ERP.Domain.Models.Entities.Account.Roles;

namespace ERP.Application.Services.Account;

public interface IRoleService: IBaseSettingService<Role,RoleCreateCommand,RoleUpdateCommand>{}