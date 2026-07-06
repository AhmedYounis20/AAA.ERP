using Accounting.Domain.Models.Entities.Roles;
using ERP.BuildingBlocks.Application.CQRS.Commands;
using ERP.BuildingBlocks.Application.Responses;
using Shared;

namespace Accounting.Application.Commands.Roles;

public record RoleDeleteCommand(Guid Id) : ICommand<ApiResponse<Role>>, IEntityDeleteCommand;
