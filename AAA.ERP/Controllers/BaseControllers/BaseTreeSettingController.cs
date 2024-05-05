using AAA.ERP.InputModels.BaseInputModels;
using AAA.ERP.Models.BaseEntities;
using AAA.ERP.Resources;
using AAA.ERP.Responses;
using AAA.ERP.Services.BaseServices.Interfaces;
using AAA.ERP.Validators.InputValidators.BaseValidators;
using AutoMapper;
using Microsoft.Extensions.Localization;

namespace AAA.ERP.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BaseTreeSettingController<TEntity, TInput> : BaseSettingController<TEntity, TInput> where TEntity : BaseTreeSettingEntity<TEntity> where TInput : BaseTreeSettingInputModel
{
    //[HttpGet]
    //public 
    private readonly IBaseTreeSettingService<TEntity> _service;
    public BaseTreeSettingController(IBaseTreeSettingService<TEntity> service, BaseSettingInputValidator<TInput> validator, IStringLocalizer<Resource> localizaer, IMapper mapper) : base(service, validator, localizaer, mapper)
    => _service = service;


    [HttpGet("GetLevel")]
    public virtual async Task<IActionResult> GetLevel([FromQuery] int level = 0)
    => Ok(new ApiResponse { IsSuccess = true, Result = await _service.GetLevel(level), StatusCode = HttpStatusCode.OK });

    [HttpGet("GetChildren/{parentId}")]
    public virtual async Task<IActionResult> GetChildren(Guid parentId, [FromQuery] int level = 0)
    => Ok(new ApiResponse { IsSuccess = true, Result = await _service.GetChildren(parentId,level), StatusCode = HttpStatusCode.OK });

}