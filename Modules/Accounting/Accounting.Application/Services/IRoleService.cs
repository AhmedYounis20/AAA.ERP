using Accounting.Application.Commands.Roles;
using ERP.BuildingBlocks.Application.Services.BaseServices;
using Accounting.Domain.Models.Entities.Roles;

namespace Accounting.Application.Services;

/// <summary>
/// Application service contract for role.
/// </summary>
public interface IRoleService : IBaseSettingService<Role, RoleCreateCommand, RoleUpdateCommand>;
