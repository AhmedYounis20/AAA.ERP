using AAA.ERP.InputModels.BaseInputModels;
using AAA.ERP.Models.BaseEntities;
using AAA.ERP.Resources;
using AAA.ERP.Services.BaseServices.interfaces;
using AAA.ERP.Validators.InputValidators.BaseValidators;
using AutoMapper;
using Microsoft.Extensions.Localization;

namespace AAA.ERP.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BaseSettingController<TEntity, TInput> : BaseController<TEntity, TInput> where TEntity : BaseSettingEntity where TInput : BaseSettingInputModel

{
    //[HttpGet]
    //public 
    private readonly IBaseSettingService<TEntity> _service;
    public BaseSettingController(IBaseSettingService<TEntity> service, BaseSettingInputValidator<TInput> validator,IStringLocalizer<Resource> localizaer, IMapper mapper) : base(service, validator, localizaer, mapper)
    => _service = service;

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string query)
    {
        var result = await _service.SearchByName(query);
        return StatusCode((int)result.StatusCode, result);
    }
}