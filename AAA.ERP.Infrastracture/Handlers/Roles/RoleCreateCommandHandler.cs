using Domain.Account.Commands.Roles;
using Domain.Account.Models.Entities.Roles;

namespace ERP.Infrastracture.Handlers.Roles;

public class RoleCreateCommandHandler(IRoleService service): ICommandHandler<RoleCreateCommand,ApiResponse<Role>>
{
    public async Task<ApiResponse<Role>> Handle(RoleCreateCommand request,
        CancellationToken cancellationToken)
    {
        return  await service.Create(request);
    }
}