using Accounting.Application.Commands.Roles;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.Roles;

namespace Accounting.Infrastructure.Handlers.Roles;

public class RoleDeleteCommandHandler(IRoleService service)
    : ICommandHandler<RoleDeleteCommand, ApiResponse<Role>>
{
    public Task<ApiResponse<Role>> Handle(RoleDeleteCommand request, CancellationToken cancellationToken)
        => service.Delete(request.Id);
}
