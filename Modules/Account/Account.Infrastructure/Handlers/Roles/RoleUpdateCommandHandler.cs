using Account.Domain.Commands.Roles;
using Account.Domain.Models.Entities.Roles;

namespace Account.Infrastructure.Handlers.Roles;

public class RoleUpdateCommandHandler(IRoleService service) : ICommandHandler<RoleUpdateCommand, ApiResponse<Role>>
{
    public async Task<ApiResponse<Role>> Handle(RoleUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}
