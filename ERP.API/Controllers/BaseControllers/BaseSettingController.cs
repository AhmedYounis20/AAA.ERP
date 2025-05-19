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
    IStringLocalizer<Resource> _localizer;
    public BaseSettingController(
        IBaseSettingService<TEntity, TCreate, TUpdate> service,
        IStringLocalizer<Resource> localizer,
        ISender sender) : base(service, localizer, sender)
    {
        _service = service;
        _sender = sender;
        _localizer = localizer;
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
            string operation = _localizer["Added"].Value;
            StringBuilder message = new StringBuilder(operation);
            message.Append(' ');
            message.Append(CurrentLanguage == "en" ? result.Result?.NameSecondLanguage : result.Result?.Name);
            message.Append(' ');
            message.Append(_localizer["Successfully"].Value);

            result.SuccessMessage = message.ToString();
        }
        result.ErrorMessages = result.ErrorMessages?.Select(e => _localizer[e].Value).ToList();
        return StatusCode((int)result.StatusCode, result);
    }

    protected override async Task<IActionResult> UpdateRecord(Guid id, TUpdate input)
    {
        input.Id = id;
        var result = await _sender.Send(input);
        if (result.IsSuccess)
        {
            string operation = _localizer["Updated"].Value;
            StringBuilder message = new StringBuilder(operation);
            message.Append(' ');
            message.Append(CurrentLanguage == "en" ? result.Result?.NameSecondLanguage : result.Result?.Name);
            message.Append(' ');
            message.Append(_localizer["Successfully"].Value);

            result.SuccessMessage = message.ToString();
        }
        result.ErrorMessages = result.ErrorMessages?.Select(e => _localizer[e].Value).ToList();
        return StatusCode((int)result.StatusCode, result);
    }

    protected override async Task<IActionResult> DeleteRecord(Guid id)
    {
        var result = await _service.Delete(id);
        if (result.IsSuccess)
        {
            string operation = _localizer["Deleted"].Value;
            StringBuilder message = new StringBuilder(operation);
            message.Append(' ');
            message.Append(CurrentLanguage == "en" ? result.Result?.NameSecondLanguage : result.Result?.Name);
            message.Append(' ');
            message.Append(_localizer["Successfully"].Value);

            result.SuccessMessage = message.ToString();
        }
        result.ErrorMessages = result.ErrorMessages?.Select(e => _localizer[e].Value).ToList();
        return StatusCode((int)result.StatusCode, result);
    }
}