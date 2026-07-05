using Accounting.Application.Commands.Roles;
using ERP.BuildingBlocks.Application.Services.BaseServices;
using Accounting.Domain.Models.Entities.Roles;

namespace Accounting.Application.Services;

public interface IRoleService : IBaseSettingService<Role, RoleCreateCommand, RoleUpdateCommand>;
