using Accounting.Application.Commands.Roles;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.Roles;
using ERP.Infrastructure.Services.Queries;

namespace Accounting.Infrastructure.Services;

/// <summary>
/// Role read queries. Transitional: delegates to <see cref="IRoleService"/> until write methods are removed.
/// </summary>
public class RoleQueries(IRoleService roleService)
    : BaseSettingQueries<Role, RoleCreateCommand, RoleUpdateCommand>(roleService),
      IRoleQueries;
