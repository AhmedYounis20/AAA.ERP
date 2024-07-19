using Domain.Account.Commands.Roles;
using Domain.Account.Models.Entities.Roles;
using Domain.Account.Services.Interfaces;
using Shared;
using Shared.Responses;

namespace Domain.Account.Handlers.Roles;

public class RoleCreateCommandHandler(IRoleService service): ICommandHandler<RoleCreateCommand,ApiResponse<Role>>
{
    public async Task<ApiResponse<Role>> Handle(RoleCreateCommand request,
        CancellationToken cancellationToken)
    {
        return  await service.Create(request);
    }
}