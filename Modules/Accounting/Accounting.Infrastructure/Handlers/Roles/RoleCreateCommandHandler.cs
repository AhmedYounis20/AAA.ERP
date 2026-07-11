using Accounting.Application.Commands.Roles;
using Accounting.Application.Repositories;
using Accounting.Domain.Models.Entities.Roles;
using ERP.BuildingBlocks.Application.CQRS.Handlers.Setting;

namespace Accounting.Infrastructure.Handlers.Roles;

public sealed class RoleCreateCommandHandler(IRoleRepository repository)
    : BaseSettingCreateHandler<Role, RoleCreateCommand>(repository);
