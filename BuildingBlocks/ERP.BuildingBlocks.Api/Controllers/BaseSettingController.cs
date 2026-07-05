using ERP.BuildingBlocks.Application.Commands.BaseInputModels.BaseCreateCommands;
using ERP.BuildingBlocks.Application.Commands.BaseInputModels.BaseUpdateCommands;
using ERP.BuildingBlocks.Application.Services.BaseServices;
using ERP.BuildingBlocks.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ERP.BuildingBlocks.Api.Controllers;

/// <summary>
/// CRUD base for named setting entities with optional name search.
/// </summary>
/// <remarks>Obsolete — see <see cref="BaseController{TEntity,TCreate,TUpdate}"/>.</remarks>
[Obsolete("Retired in hybrid CQRS (ADR-012). See HYBRID_CQRS_PLAN.md.")]
[Route("api/[controller]")]
[ApiController]
public class BaseSettingController<TEntity, TCreate, TUpdate>
    : BaseController<TEntity, TCreate, TUpdate>
    where TEntity : BaseSettingEntity
    where TCreate : BaseSettingCreateCommand<TEntity>
    where TUpdate : BaseSettingUpdateCommand<TEntity>
{
    private readonly IBaseSettingService<TEntity, TCreate, TUpdate> _service;

    public BaseSettingController(
        IBaseSettingService<TEntity, TCreate, TUpdate> service,
        ISender sender) : base(service, sender)
        => _service = service;

    /// <summary>Searches settings by name (primary or second language).</summary>
    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string query)
    {
        var result = await _service.SearchByName(query);
        return StatusCode((int)result.StatusCode, result);
    }
}
