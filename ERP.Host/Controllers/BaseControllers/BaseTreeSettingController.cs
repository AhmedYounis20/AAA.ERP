using ERP.BuildingBlocks.Application.Commands.BaseInputModels.BaseCreateCommands;
using ERP.BuildingBlocks.Application.Commands.BaseInputModels.BaseUpdateCommands;
using ERP.BuildingBlocks.Application.Responses;

namespace ERP.Host.Controllers.BaseControllers;

/// <summary>
/// CRUD base for hierarchical setting entities (tree level and children queries).
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class BaseTreeSettingController<TEntity, TCreate, TUpdate>
    : BaseSettingController<TEntity, TCreate, TUpdate>
    where TEntity : BaseTreeSettingEntity<TEntity>
    where TCreate : BaseTreeSettingCreateCommand<TEntity>
    where TUpdate : BaseTreeSettingUpdateCommand<TEntity>
{
    //[HttpGet]
    //public 
    private readonly IBaseTreeSettingService<TEntity, TCreate, TUpdate> _service;
    public BaseTreeSettingController(IBaseTreeSettingService<TEntity, TCreate, TUpdate> service, ISender sender)
        : base(service, sender)
    {
        _service = service;
    }

    /// <summary>Gets all nodes at the given tree depth (root level when <paramref name="level"/> is 0).</summary>
    [HttpGet("GetLevel")]
    public virtual async Task<IActionResult> GetLevel([FromQuery] int level = 0)
    => Ok(new ApiResponse { IsSuccess = true, Result = await _service.GetLevel(level), StatusCode = HttpStatusCode.OK });

    /// <summary>Gets child nodes under <paramref name="parentId"/> down to the requested depth.</summary>
    [HttpGet("GetChildren/{parentId}")]
    public virtual async Task<IActionResult> GetChildren(Guid parentId, [FromQuery] int level = 0)
    => Ok(new ApiResponse { IsSuccess = true, Result = await _service.GetChildren(parentId, level), StatusCode = HttpStatusCode.OK });
}
