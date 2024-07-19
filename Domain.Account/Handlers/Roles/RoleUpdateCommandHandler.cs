using Domain.Account.Commands.Roles;
using Domain.Account.Models.Entities.Roles;
using Domain.Account.Services.Interfaces;
using Shared;
using Shared.Responses;

namespace Domain.Account.Handlers.Roles;

public class RoleUpdateCommandHandler(IRoleService service): ICommandHandler<RoleUpdateCommand,ApiResponse<Role>>
{
    public async Task<ApiResponse<Role>> Handle(RoleUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}