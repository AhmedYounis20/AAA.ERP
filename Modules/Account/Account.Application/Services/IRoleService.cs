using Account.Domain.Commands.Roles;
using Account.Domain.Models.Entities.Roles;
using Shared.Application.BaseServices;
using Shared.Domain.BaseEntities;
using Shared.Domain.Responses;

namespace Account.Application.Services;

public interface IRoleService: IBaseSettingService<Role,RoleCreateCommand,RoleUpdateCommand>{}