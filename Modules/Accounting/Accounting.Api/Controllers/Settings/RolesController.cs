using Accounting.Application.Commands.Roles;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.Roles;
using ERP.BuildingBlocks.Api.Controllers;
using ERP.BuildingBlocks.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Accounting.Api.Controllers.Settings;

/// <summary>CRUD API for job role settings (hybrid CQRS).</summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class RolesController(ISender sender, IRoleQueries queries)
    : HybridSettingController<Role, RoleCreateCommand, RoleUpdateCommand>(sender, queries)
{
    protected override IRequest<ApiResponse<Role>> BuildDeleteCommand(Guid id)
        => new RoleDeleteCommand(id);
}
