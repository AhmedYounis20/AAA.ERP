using Domain.Account.Commands.BaseInputModels.BaseCreateCommands;
using Domain.Account.Commands.BaseInputModels.BaseUpdateCommands;
using Domain.Account.Services.BaseServices.interfaces;
using MediatR;
using Microsoft.Extensions.Localization;
using Shared.BaseEntities;
using Shared.Resources;
using Shared.Responses;

namespace AAA.ERP.Controllers.BaseControllers;

[Route("api/[controller]")]
[ApiController]
public class BaseTreeSettingController<TEntity, TCreate, TUpdate> 
    : BaseSettingController<TEntity, TCreate,TUpdate> 
    where TEntity : BaseTreeSettingEntity<TEntity> 
    where TCreate : BaseTreeSettingCreateCommand<ApiResponse<TEntity>>
    where TUpdate : BaseTreeSettingUpdateCommand<ApiResponse<TEntity>>
{
    //[HttpGet]
    //public 
    private readonly IBaseTreeSettingService<TEntity> _service;
    private readonly IStringLocalizer<Resource> _localizer;
    public BaseTreeSettingController(IBaseTreeSettingService<TEntity> service, IStringLocalizer<Resource> localizer, ISender sender) 
        : base(service, localizer, sender)
    {
        _service = service;
        _localizer = localizer;
    }

    [HttpGet("GetLevel")]
    public virtual async Task<IActionResult> GetLevel([FromQuery] int level = 0)
    => Ok(new ApiResponse { IsSuccess = true, Result = await _service.GetLevel(level), StatusCode = HttpStatusCode.OK });

    [HttpGet("GetChildren/{parentId}")]
    public virtual async Task<IActionResult> GetChildren(Guid parentId, [FromQuery] int level = 0)
    => Ok(new ApiResponse { IsSuccess = true, Result = await _service.GetChildren(parentId,level), StatusCode = HttpStatusCode.OK });

    protected override async Task<IActionResult> DeleteRecord(Guid id)
    {

        var result = await _service.Delete(id);
        result.ErrorMessages = result.ErrorMessages?.Select(e => _localizer[e].Value).ToList();

        return StatusCode((int)result.StatusCode, result);
    }
}