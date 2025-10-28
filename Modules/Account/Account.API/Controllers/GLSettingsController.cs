using Account.Domain.Commands;
using Account.Domain.Commands.GLSettings;
using Account.Domain.Models.Entities.GLSettings;
using Mapster;
using Microsoft.AspNetCore.Authorization;

namespace Account.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class GLSettingsController : ControllerBase
{
    //GLSettingInputValidator _validator;
    IGLSettingService _service;

    public string CurrentLanguage => HttpContext.Request.Headers.ContainsKey("Accept-Language") &&
                                       HttpContext.Request.Headers["Accept-Language"].Any(e => e.Contains("ar")) ||
                                      HttpContext.Request.Headers.ContainsKey("Accept-Culture") &&
                                       HttpContext.Request.Headers["Accept-Culture"].Any(e => e.Contains("ar"))
        ? "ar"
        : "en";

    public GLSettingsController(IGLSettingService service)
        //GLSettingInputValidator validator,)
    {
        //_validator = validator;
        _service = service;
    }

    [HttpGet]
    public virtual async Task<IActionResult> Get()
    {
        var result = await _service.Get();
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpPut]
    public virtual async Task<IActionResult> Update(GlSettingUpdateCommand input)
    {
        var toValidate = input.Adapt<GLSettingInputModel>();
        //var validationResult = _validator.Validate(toValidate);
        //if (validationResult.IsValid)
        //{
            var entity = input.Adapt<GLSetting>();
            //var userId = User.Claims.FirstOrDefault(e => e.Type == ClaimTypes.NameIdentifier)?.Value;
            //entity.ModifiedBy = Guid.Parse(userId ?? "");
            var result = await _service.Update(input);

            //if (result.IsSuccess)
            //{
            //    string operation = _localizer["Updated"].Value;
            //    StringBuilder message = new StringBuilder(operation);
            //    message.Append(' ');
            //    message.Append(_localizer["GLSettings"]);
            //    message.Append(' ');
            //    message.Append(_localizer["Successfully"].Value);

            //    result.SuccessMessage = message.ToString();
            //}

            //result.ErrorMessages = result.ErrorMessages?.Select(e => _localizer[e].Value).ToList();

            return StatusCode((int)result.StatusCode, result);
        //}
        //else
        //{
        //    return BadRequest(
        //        new ApiResponse<GLSetting>
        //        {
        //            IsSuccess = false,
        //            StatusCode = HttpStatusCode.BadRequest,
        //            //ErrorMessages = validationResult.Errors.Select(e => _localizer[e.ErrorMessage].Value).ToList(),
        //        });
        //}
    }
}
