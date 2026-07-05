using Accounting.Application.Commands.GLSettings;
using Accounting.Application.Services;
using Accounting.Application.Validators.GLSettings;
using Accounting.Domain.Commands;
using Accounting.Domain.Models.Entities.GLSettings;
using Mapster;
using Shared.Responses;
using Shared.Constants;

namespace ERP.Host.Controllers.Account;

/// <summary>Read/update API for general ledger settings.</summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class GLSettingsController : ControllerBase
{
    GLSettingInputValidator _validator;
    IGLSettingService _service;

    public string CurrentLanguage => HttpContext.Request.Headers.ContainsKey("Accept-Language") &&
                                       HttpContext.Request.Headers["Accept-Language"].Any(e => e.Contains("ar")) ||
                                      HttpContext.Request.Headers.ContainsKey("Accept-Culture") &&
                                       HttpContext.Request.Headers["Accept-Culture"].Any(e => e.Contains("ar"))
        ? "ar"
        : "en";

    public GLSettingsController(IGLSettingService service,
        GLSettingInputValidator validator)
    {
        _validator = validator;
        _service = service;
    }

    /// <summary>Lists all records.</summary>
    [HttpGet]
    public virtual async Task<IActionResult> Get()
    {
        var result = await _service.Get();
        return StatusCode((int)result.StatusCode, result);
    }

    /// <summary>Updates an existing record.</summary>
    [HttpPut]
    public virtual async Task<IActionResult> Update(GlSettingUpdateCommand input)
    {
        var toValidate = input.Adapt<GLSettingInputModel>();
        var validationResult = _validator.Validate(toValidate);
        if (validationResult.IsValid)
        {
            var entity = input.Adapt<GLSetting>();
            var userId = User.Claims.FirstOrDefault(e => e.Type == ClaimTypes.NameIdentifier)?.Value;
            entity.ModifiedBy = Guid.Parse(userId ?? "");
            var result = await _service.Update(input);

            if (result.IsSuccess)
            {
                result.Success = new MessageTemplate { MessageKey = SuccessMessages.UpdatedSuccessfully };
            }

            return StatusCode((int)result.StatusCode, result);
        }
        else
        {
            return BadRequest(
                new ApiResponse<GLSetting>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Errors = validationResult.Errors.Select(e => new MessageTemplate { MessageKey = e.ErrorMessage }).ToList(),
                });
        }
    }
}
