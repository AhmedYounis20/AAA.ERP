using Domain.Account.Commands.BaseInputModels.BaseCreateCommands;
using Domain.Account.Commands.BaseInputModels.BaseUpdateCommands;
using Shared.Responses;

namespace ERP.API.Controllers.BaseControllers;

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

    [HttpGet("GetLevel")]
    public virtual async Task<IActionResult> GetLevel([FromQuery] int level = 0)
    => Ok(new ApiResponse { IsSuccess = true, Result = await _service.GetLevel(level), StatusCode = HttpStatusCode.OK });

    [HttpGet("GetChildren/{parentId}")]
    public virtual async Task<IActionResult> GetChildren(Guid parentId, [FromQuery] int level = 0)
    => Ok(new ApiResponse { IsSuccess = true, Result = await _service.GetChildren(parentId, level), StatusCode = HttpStatusCode.OK });
}