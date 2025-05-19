using Domain.Account.Commands.Roles;
using Domain.Account.Models.Entities.Roles;

namespace ERP.Infrastracture.Handlers.Roles;

public class RoleUpdateCommandHandler(IRoleService service): ICommandHandler<RoleUpdateCommand,ApiResponse<Role>>
{
    public async Task<ApiResponse<Role>> Handle(RoleUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}