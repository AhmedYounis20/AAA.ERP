using ERP.Domain.Commands.Account.Roles;
using ERP.Domain.Models.Entities.Account.Roles;

namespace ERP.Infrastracture.Handlers.Account.Roles;

public class RoleUpdateCommandHandler(IRoleService service) : ICommandHandler<RoleUpdateCommand, ApiResponse<Role>>
{
    public async Task<ApiResponse<Role>> Handle(RoleUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}