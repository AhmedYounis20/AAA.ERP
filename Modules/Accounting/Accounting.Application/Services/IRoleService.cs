using Accounting.Application.Commands.Roles;
using ERP.Application.Services.BaseServices;
using ERP.Domain.Models.Entities.Account.Roles;

namespace Accounting.Application.Services;

public interface IRoleService : IBaseSettingService<Role, RoleCreateCommand, RoleUpdateCommand>;
