using ERP.Domain.Commands.Account.Roles;
using ERP.Domain.Models.Entities.Account.Roles;

namespace ERP.Infrastracture.Handlers.Account.Roles;

public class RoleCreateCommandHandler(IRoleService service) : ICommandHandler<RoleCreateCommand, ApiResponse<Role>>
{
    public async Task<ApiResponse<Role>> Handle(RoleCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}