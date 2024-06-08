using AAA.ERP.Validators.InputValidators.BaseValidators;
using AutoMapper;
using Domain.Account.Commands.BaseInputModels.BaseCreateCommands;
using Domain.Account.Commands.BaseInputModels.BaseUpdateCommands;
using Domain.Account.InputModels.BaseInputModels;
using Domain.Account.Services.BaseServices.interfaces;
using MediatR;
using Microsoft.Extensions.Localization;
using Shared.BaseEntities;
using Shared.Resources;
using Shared.Responses;

namespace AAA.ERP.Controllers.BaseControllers;

[Route("api/[controller]")]
[ApiController]
public class BaseSettingController<TEntity, TCreate, TUpdate>
    : BaseController<TEntity, TCreate, TUpdate>
    where TEntity : BaseSettingEntity
    where TCreate : BaseSettingCreateCommand<TEntity>
    where TUpdate : BaseSettingUpdateCommand<TEntity>
{
    //[HttpGet]
    //public 
    private readonly IBaseSettingService<TEntity, TCreate, TUpdate> _service;

    public BaseSettingController(
        IBaseSettingService<TEntity, TCreate, TUpdate> service,
        IStringLocalizer<Resource> localizaer,
        ISender sender) : base(service, localizaer, sender)
        => _service = service;

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string query)
    {
        var result = await _service.SearchByName(query);
        return StatusCode((int) result.StatusCode, result);
    }
}