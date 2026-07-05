using Accounting.Application.Commands.Roles;
using Accounting.Application.Services;
using ERP.Domain.Models.Entities.Account.Roles;

namespace Accounting.Infrastructure.Handlers.Roles;

public class RoleUpdateCommandHandler(IRoleService service)
    : ICommandHandler<RoleUpdateCommand, ApiResponse<Role>>
{
    public Task<ApiResponse<Role>> Handle(RoleUpdateCommand request, CancellationToken cancellationToken)
        => service.Update(request);
}
