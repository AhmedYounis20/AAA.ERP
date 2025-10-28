using Domain.Account.Commands.BaseInputModels.BaseCreateCommands;
using Domain.Account.Commands.BaseInputModels.BaseUpdateCommands;

namespace ERP.API.Controllers.BaseControllers;

[Route("api/[controller]")]
[ApiController]
public class BaseSettingController<TEntity, TCreate, TUpdate>
    : BaseController<TEntity, TCreate, TUpdate>
    where TEntity : BaseSettingEntity
    where TCreate : BaseSettingCreateCommand<TEntity>
    where TUpdate : BaseSettingUpdateCommand<TEntity>
{

    private readonly IBaseSettingService<TEntity, TCreate, TUpdate> _service;
    private readonly ISender _sender;
    public BaseSettingController(
        IBaseSettingService<TEntity, TCreate, TUpdate> service,
        ISender sender) : base(service, sender)
    {
        _service = service;
        _sender = sender;
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string query)
    {
        var result = await _service.SearchByName(query);
        return StatusCode((int)result.StatusCode, result);
    }

    protected override async Task<IActionResult> CreateRecord(TCreate input)
    {
        var result = await _sender.Send(input);
        if (result.IsSuccess)
        {
            result.Success = new MessageTemplate
            {
                MessageKey = "AddedSuccessfullyWithName",
                Args = new object?[] { CurrentLanguage == "en" ? result.Result?.NameSecondLanguage : result.Result?.Name }!
            };
        }
        return StatusCode((int)result.StatusCode, result);
    }

    protected override async Task<IActionResult> UpdateRecord(Guid id, TUpdate input)
    {
        input.Id = id;
        var result = await _sender.Send(input);
        if (result.IsSuccess)
        {
            result.Success = new MessageTemplate
            {
                MessageKey = "UpdatedSuccessfullyWithName",
                Args = new object?[] { CurrentLanguage == "en" ? result.Result?.NameSecondLanguage : result.Result?.Name }!
            };
        }
        return StatusCode((int)result.StatusCode, result);
    }

    protected override async Task<IActionResult> DeleteRecord(Guid id)
    {
        var result = await _service.Delete(id);
        if (result.IsSuccess)
        {
            result.Success = new MessageTemplate
            {
                MessageKey = "DeletedSuccessfullyWithName",
                Args = new object?[] { CurrentLanguage == "en" ? result.Result?.NameSecondLanguage : result.Result?.Name }!
            };
        }
        return StatusCode((int)result.StatusCode, result);
    }
}