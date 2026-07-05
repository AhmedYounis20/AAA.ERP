using Accounting.Application.Commands.Roles;
using Accounting.Application.Services;
using ERP.Domain.Models.Entities.Account.Roles;

namespace Accounting.Infrastructure.Handlers.Roles;

public class RoleCreateCommandHandler(IRoleService service)
    : ICommandHandler<RoleCreateCommand, ApiResponse<Role>>
{
    public Task<ApiResponse<Role>> Handle(RoleCreateCommand request, CancellationToken cancellationToken)
        => service.Create(request);
}
